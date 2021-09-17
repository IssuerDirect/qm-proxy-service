var alertlist = {};
$(function () {
    alertlist = new Vue({
        el: "#alerts",
        data: {
            fullList: alertsListData.data,
            action: "unhide",
            categoryId: -90,
            keywords: "",
            recs: [],
            selectAll: false,
            msgBox: "",
            totalCount: alertsListData.count,
            currentPage: 0
        },
        computed: {
            title: function () { 
                return 'SNN Alerts';
            },
            showloader: function () {
                return this.totalCount > this.fullList.length;
            }
        },
        methods: {
            selectionChange: function () {
                this.recs = [];
                if (this.selectAll) {
                    for (pak of this.fullList) {
                        this.recs.push(pak.id);
                    }
                }
            },
            search: async function () {

                this.currentPage = 0;
                var items = await (await net3000.common.handlePromise({
                    apiurl: `/admin/Alerts/?pageIndex=0&keywords=${this.keywords}&categoryId=${this.categoryId}&json=true`
                })).json();
                this.fullList = items.data;
                this.totalCount = items.count;
            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage += 1;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/admin/Alerts/?pageIndex=${this.currentPage}&keywords=${this.keywords}&categoryId=${this.categoryId}&json=true`
                })).json();
                this.fullList = this.fullList.concat(nextPage.data);
            },
            async deletealert(item = null) {
                let confirmResult = await Swal.fire({
                    title: 'Are you sure?',
                    text: `You're about to delete client# ${this.recs.length > 0 ? this.recs.join() : item.id}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Delete'
                });
                if (confirmResult.value) {
                    this.processing = true;
                    var res = await (await net3000.common.handlePromise({ apiurl: `/admin/alert/${this.recs.length > 0 ? this.recs.join() : item.id}`, method: "Delete" })).json();
                    this.msgBox = res.html;
                    this.processing = false;
                    this.fullList = this.fullList.filter(c => c.id != item.id);
                    this.recs = [];
                }

            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;"; }
            }
        }
    });

});
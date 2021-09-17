var insightlist = {};
$(function () {
    insightlist = new Vue({
        el: "#insights",
        data: {
            fullList: insightsListData.data,
            action: "unhide",
            typeId: 0,
            statusId: -90,
            keywords: "",
            recs: [],
            selectAll: false,
            msgBox: "",
            totalCount: insightsListData.count,
            currentPage: 0
        },
        computed: {
            title: function () {
                //var sts = `${this.selectedStatus} ${this.packageTypes[this.packageType]} Packages`;
                //if (this.packageType == 0) {
                //    sts = "All " + sts;
                //}                          
                //return sts;
                return 'SNN Insights';
            },
            showloader: function () {
                return this.totalCount > this.fullList.length ;
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
            search: async  function () {

                    this.currentPage = 0;
                    var items = await (await net3000.common.handlePromise({
                        apiurl: `/admin/Insights/?pageIndex=0&keywords=${this.keywords}&Type=${this.typeId}&status=${this.statusId}&json=true`
                    })).json();
             this.fullList = items.data;
                    this.totalCount = items.count;
            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage+=1;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/admin/Insights/?pageIndex=${this.currentPage}&keywords=${this.keywords}&Type=${this.typeId}&status=${this.statusId}&json=true`
                })).json();
                this.fullList =this.fullList.concat(nextPage.data);
            },
            delete: async function (item) {
                let confirmResult = await Swal.fire({
                    title: 'Are you sure?',
                    text: `You're about to delete client# ${item.id} - ${item.title}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Delete'
                });
                if (confirmResult.value) {
                    this.processing = true;
                    var res = await (await net3000.common.handlePromise({ apiurl: `/admin/Insight/${item.id}`, method: "Delete" })).json();
                    this.msgBox = res.html;
                    this.processing = false;
                    this.fullList = this.fullList.filter(c => c.id != item.id);
                }
            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;"; }
            }
        }
    });    

});
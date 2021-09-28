var videolist = {};
$(function () {
    videolist = new Vue({
        el: "#videos",
        data: {
            fullList: videosListData.data,
            action: "delete",
            categoryId: 0,
            keywords: "",
            recs: [],
            selectAll: false,
            msgBox: "",
            totalCount: videosListData.count,
            currentPage: 0
        },
        computed: {
            showloader: function () {
                return this.totalCount > this.fullList.length;
            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;"; }
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
            dateFormat: function (date) {
                return moment(date).format("DD MMM YYYY");
            },
            search: async function () {

                this.currentPage = 0;
                var items = await (await net3000.common.handlePromise({
                    apiurl: `/admin/videos/?pageIndex=0&keywords=${this.keywords}&json=true`
                })).json();
                this.fullList = items.data;
                this.totalCount = items.count;
            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage += 1;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/admin/videos/?pageIndex=${this.currentPage}&keywords=${this.keywords}&json=true`
                })).json();
                this.fullList = this.fullList.concat(nextPage.data);
                this.totalCount = this.fullList.count;
            },
            async deletevideo(item = null) {
                let confirmResult = await Swal.fire({
                    title: 'Are you sure?',
                    text: `You're about to delete video# ${this.recs.length > 0 ? this.recs.join() : item.id}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Delete'
                });
                if (confirmResult.value) {
                    this.processing = true;
                    var res = await (await net3000.common.handlePromise({ apiurl: `/admin/video?ids=${this.recs.length > 0 ? this.recs.join() : item.id}`, method: "Delete" })).json();
                    this.msgBox = res.html;
                    this.processing = false;
                    if (this.recs.length > 0) {
                        this.fullList = this.fullList.filter(c => !this.recs.includes(c.id));
                        this.totalCount = this.totalCount - this.recs.length;
                        this.recs =  [];
                    }
                    else {
                        this.fullList = this.fullList.filter(c => c.id != item.id);
                        this.totalCount = this.totalCount - 1;
                    } 
                } 
            }
        }
    });

});
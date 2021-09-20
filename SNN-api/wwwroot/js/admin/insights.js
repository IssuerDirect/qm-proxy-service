var insightlist = {};
$(function () {
    insightlist = new Vue({
        el: "#insights",
        data: {
            fullList: insightsListData.data,
            action: "unhide",
            typeId: null,
            statusId: null,
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
            dateFormat: function (dt) {
                if (!dt) { return null; }
                return moment(dt).format("LLLL");
            },
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
           async deleteInsight(item=null) {
                let confirmResult = await Swal.fire({
                    title: 'Are you sure?',
                    text: `You're about to delete client# ${this.recs.length > 0? this.recs.join() : item.id}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Delete'
                });
                if (confirmResult.value) {
                    this.processing = true;
                    var res = await (await net3000.common.handlePromise({ apiurl: `/admin/Insight?ids=${this.recs.length > 0? this.recs.join() : item.id}`, method: "Delete" })).json();
                    this.msgBox = res.html;
                    this.processing = false;
                    if (this.recs.length > 0)
                        this.fullList = this.fullList.filter(c => !this.recs.includes(c.id));
                    else
                        this.fullList = this.fullList.filter(c => c.id != item.id);
                    this.recs = new [];
                    this.totalCount = this.fullList.count;
               }

            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;"; }
            }
        }
    });    

});
var insightlist = {};
$(function () {
    insightlist = new Vue({
        el: "#insights",
        data: {
            fullList: insightsListData.data,
            insightsList: [],
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
            displayedInsights: function () {
                this.insightsList = this.fullList;


              
                return this.insightsList;
            },
            showloader: function () {
                return this.totalCount > this.fullList.length ;
            }
        },
        methods: {
            selectionChange: function () {
                this.recs = [];
                if (this.selectAll) {
                    for (pak of this.insightsList) {
                        this.recs.push(pak.id);
                    }
                }
            },
            search: function (includeKeyword = false) {
                this.insightsList = this.fullList.data;

            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage+=1;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/admin/Insights/?pageIndex=${this.currentPage}&json=true`
                })).json();
                this.typeId = 0;
                this.statusId = -90;
                this.keywords = null;
                this.fullList =this.fullList.concat(nextPage.data);
                this.insightsList = this.fullList;
            },
            takeAction: async function () {
                this.msgBox = null;
                if (this.action == null) {
                    return;
                } else if (this.action == "delete") {
                    var res = await (await net3000.common.handlePromise({
                        apiurl: `/admin/Insights?ids=${this.recs.join(',')}`,
                        method: "delete"
                    })).json();
                    this.fullList = this.fullList.filter(r => !this.recs.contains(r.id));
                    this.recs = [];
                    this.msgBox = res;
                }
            },
            deleteConfirmation: function (myResponse) {
                this.msgBox = myResponse.html;
                this.fullList = this.fullList.filter(p => !this.recs.includes(p.id));
                this.recs = [];
            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;"; }
            }
        },
        mounted: function () {
            this.insightsList = this.fullList;
        }
    });    

});
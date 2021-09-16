var insightlist = {};
$(function () {
    insightlist = new Vue({
        el: "#insights",
        data: {
            fullList: insightsListData,
            insightsList: [],
            action: "unhide",
            typeId: 0,
            statusId: -90,
            keywords: "",
            recs: [],
            selectAll: false,
            msgBox: "",
            totalCount: totalCount,
            currentPage: 0,
            title:"Insights"
        },
        computed: {
            title: function () {
                var sts = `${this.selectedStatus} ${this.packageTypes[this.packageType]} Packages`;
                if (this.packageType == 0) {
                    sts = "All " + sts;
                }                          
                return sts;
            },
            displayedInsights: function () {
                this.insightsList = this.fullList;

                
                if (this.keywords != null) {
                    this.insightsList = this.insightsList.filter(p => (p.title != null && p.title.toLowerCase().includes(this.keywords.toLowerCase())) );
                }
                if (this.typeId && this.typeId != 0) {
                    this.insightsList = this.insightsList.filter(p => p.type === this.typeId);
                }
                if (this.statusId && this.statusId != -90) {
                    this.insightsList = this.insightsList.filter(p => p.ref_Status === this.statusId );
                }
                
                return this.insightsList;
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
                this.insightsList = this.fullList;
                if (includeKeyword != false && this.keywords != null) {
                    this.insightsList = this.insightsList.filter(p => p.title.toLowerCase().includes(this.keywords.toLowerCase()))
                }
                if (this.typeId != 0) {
                    this.insightsList = this.insightsList.filter(p => p.type === this.typeId);
                }
                if (this.statusId != -90) {
                    this.insightsList = this.insightsList.filter(p => p.ref_Status === this.statusId);
                }
            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage++;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/snn_Insights/Index?pageIndex=${this.currentPage}` 
                })).json;
                this.typeId = 0;
                this.statusId = -90;
                this.keywords = null;
                this.fullList.concat(nextPage.data);
                this.insightsList = this.fullList;
            },
            takeAction: async function () {
                this.msgBox = null;
                if (this.action == "delete") {
                    deleteOptions = {};
                    deleteOptions.title = "Delete Selected Packages?";
                    deleteOptions.html = `Are you sure you want to delete Packages:<br><span style='color:red; font-weight: bold'>${this.recs.join(',')}</span>`;
                    deleteOptions.action = `/mt/packages/delete?ids=${this.recs.join(',')}`;
                    deleteOptions.callBackFunction = this.deleteConfirmation;
                    adminConfirmDelete(deleteOptions);
                } else {
                    this.msgBox = null;
                    var newStatus = this.action == "hide" ? true : false;
                    var res = await (await net3000.common.handlePromise({
                        apiurl: `/mt/packages/changeStatus?packagesIDs=${this.recs.join(',')}&hide=${newStatus}`,
                        method: 'PUT'
                    })).json();
                    if (this.action == "hide") {
                        for (id of this.recs) {
                            var package = this.insightsList.filter(p => p.id == id)[0];
                            package.isAvailable = false;
                            package.hidePackage = true;
                        }
                    }
                    this.recs = [];
                    this.msgBox = res.html;
                }
            },
            deleteConfirmation: function (myResponse) {
                this.msgBox = myResponse.html;
                this.fullList = this.fullList.filter(p => !this.recs.includes(p.id));
                this.recs = [];
            },
            showactionBar: function () {
                if (this.recs.length > 0) { return "display: block;" } else { return "display: none;";}
            }
        },
        mounted: function() {
            this.insightsList = this.fullList;
        }
    })    

});
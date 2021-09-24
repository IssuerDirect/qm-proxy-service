var insightlist = {};
$(function () {
    insightlist = new Vue({
        el: "#insights",
        data: {
            fullList: insightsListData.data,
            action: "delete",
            typeId: null,
            keywords: "",
            recs: [],
            selectAll: false,
            msgBox: null,
            refTypeId: null,
            url: "",
            totalCount: insightsListData.count,
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
            dateFormat: function (dt) {
                if (!dt) { return null; }
                return moment(dt).format("l LT");
            },
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
                    apiurl: `/admin/Insights/?pageIndex=0&keywords=${this.keywords}&Type=${this.typeId}&json=true`,
                    method: "POST"
                })).json();
                this.fullList = items.data;
                this.totalCount = items.count;
            },
            importInsights: async function () {

                if (!document.getElementById("importform").checkValidity()) {
                    $("#importform").addClass("was-validated");
                } else {
                    $("#importform").remove("was-validated");
                    if (this.refTypeId !== null && this.url !== "") {
         $('#importModal').modal('toggle');
                        var res = await (await net3000.common.handlePromise({
                            apiurl: `/admin/Insights/import?typeID=${this.refTypeId}&url=${this.url}`, method: 'POST'
                        })).json();
                       
                            this.fullList = res.data;
                        this.currentPage = ((res.count / 24) - 1).toString().split('.')[0];
                        if (this.currentPage < 0)
                            this.currentPage = 0;
                            this.totalCount += res.count;
                        this.msgBox = res;
                        this.keywords = '';
                        this.typeId = null;
                    }
                }

            },
            loadNextPage: async function () {
                //not using this now. We're loading all account packages and filtering on page
                this.currentPage += 1;
                var nextPage = await (await net3000.common.handlePromise({
                    apiurl: `/admin/Insights/?pageIndex=${this.currentPage}&keywords=${this.keywords}&Type=${this.typeId}&json=true`
                })).json();
                this.fullList = this.fullList.concat(nextPage.data);
            },
            async deleteInsight(item = null) {
                let confirmResult = await Swal.fire({
                    title: 'Are you sure?',
                    text: `You're about to delete insight# ${this.recs.length > 0 ? this.recs.join() : item.id}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Delete'
                });
                if (confirmResult.value) {
                    this.processing = true;
                    var res = await (await net3000.common.handlePromise({ apiurl: `/admin/Insight?ids=${this.recs.length > 0 ? this.recs.join() : item.id}`, method: "Delete" })).json();
                    this.msgBox = res;
                    this.processing = false;
                    if (this.recs.length > 0) {
                        this.fullList = this.fullList.filter(c => !this.recs.includes(c.id));
                        this.totalCount = this.totalCount - this.recs.length;
                        this.recs = [];
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
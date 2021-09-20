var vm_featureImages = {};
var images = {};
var menuTop;

$(function () {

    if (sessionStorage.getItem("adminMenu") === null) {
        net3000.common.handlePromise({ apiurl: "/api/getmenu", parameters: { container: "#adminHeaderContainer", template: "#adminHeaderTemplate", callBackFunction: saveMenu } });
    } else if ($("#adminHeaderContainer").length > 0) {
        $("#adminHeaderContainer").html(Mustache.render($("#adminHeaderTemplate").html(), JSON.parse(sessionStorage.getItem("adminMenu"))));
    }

    if ($(".feature-image").length > 0) {
        mountFeatureImages();
    }

    //Start Gridview check box listeners
    $("input[type=checkbox].selectAll").change(function () {
        $(this).parents("table").find("input[type=checkbox]").prop("checked", $(this).prop("checked"));
        if ($(this).prop("checked")) {
            $(".actionButtons").fadeIn();
        } else {
            $(".actionButtons").fadeOut();
        }
    });

    document.addEventListener("scroll", fixMenu);
    $("input[name=recs]").change(function () {
        if ($(".actionButtons").length > 0) {
            if ($("input[name=recs]:checked").length > 0) {
                $(".actionButtons").fadeIn();
            } else {
                $(".actionButtons").fadeOut();
            }
        }
    });

    //End Gridview check box listeners
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({
        trigger: 'hover',
        html: true
    });
   
});

function saveMenu(myResponse) {
    sessionStorage.setItem("adminMenu", JSON.stringify(myResponse));
}

function fixMenu() {
    if (window.scrollY >= menuTop) {
        $("nav").addClass("fixed-top");
    } else if ($("nav").hasClass("fixed-top")) {
        $("nav").removeClass("fixed-top");
    }
}

var editors = [];
function createEditor(name) {
    if (editors[name] === undefined) {
        ClassicEditor.create(document.querySelector(name)).then(newEditor => { editors[name] = newEditor; });
        $(name + "editorOptions > *").toggle();
    }
}

function closeEditor(name) {
    if (editors[name] !== undefined) {
        editors[name].destroy();
        editors[name] = undefined;
        $(name + "editorOptions > *").toggle();
    }
}

//loads images to local storage, initializes VUE instances on each input with class feature-image
function mountFeatureImages() {
    if (sessionStorage.getItem("images") === null) {
        net3000.common.handlePromise({ apiurl: "/api/images", parameters: { callBackFunction: saveImages } });
    } else {
        images = JSON.parse(sessionStorage.getItem("images"));
        initializeVue();
    }
    function saveImages(myResponse) {
        sessionStorage.setItem("images", JSON.stringify(myResponse.data));
        images = myResponse.data;
        initializeVue();
    }   
}

function validURL(str) {
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    return !!pattern.test(str);
}

function friendlyLink(str) {
    var pattern = new RegExp(
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    return !!pattern.test(str);
}

function bindMyImages(myResponse) {
    for (var image of myResponse.data) {
        $(".feature-image").each(function () {
            vm_featureImages[$(this).attr("id")].$data.accountImages.unshift(image);
        });
    }
}
//parameters include id, data, action
function mountCustomFields(parameters) {
    if ($(parameters.id).length === 0) { return;}
    fetch("/templates/v-customfields.html").then((response) => {
        response.text().then((data) => {
            new Vue({
                el: parameters.id,
                template: data,
                data: {
                    customFields: parameters.data,
                    resetData: parameters.data,
                    action: parameters.action,
                    id: parameters.id
                },
                methods: {
                    addNewItem: function () {
                        $("#customFieldList").append($("#newRow").html());
                    },
                    saveCustomFields: function () {
                        let myCustomFields = {};
                        $("#customFieldList > div").each(function () {
                            if ($(this).find("input[name=key]").val() !== "") {
                                myCustomFields[$(this).find("input[name=key]").val()] = $(this).find("textarea[name=value]").val();
                            }
                        });
                        net3000.common.handlePromise({
                            apiurl: this.action, body: JSON.stringify(myCustomFields), parameters: { msgBox: `${this.id} .msgBox`, buttonContainer: `${this.id} .buttonContainer` }
                        });
                    }
                }
            });
        });
    });
}

//action, container,optional data
function saveContainer(parameters) {
    var myData = net3000.common.collectData(parameters.container);
    if (parameters.data !== undefined) {
        for (let key in parameters.data) {
            myData[key] = parameters.data[key];
        }
    }
    myData.AntiforgeryFieldname = undefined;
    net3000.common.handlePromise({
        apiurl: parameters.action,
        body: JSON.stringify(myData),
        parameters: {
            msgBox: parameters.container + " .msgBox",
            buttonContainer: parameters.container + " .buttonContainer"
        }
    });
}

function adminDeletedMessage(msg) {
    if ($("#msgBox").length > 0) {
        $("#msgBox").html(net3000.common.msgBox(msg));
    }
    if (msg.data !== undefined && msg.data !== null) {
        for (let page of msg.data.split(',')) {
            if ($(`#row-${page}`).length > 0) {
                $(`#row-${page}`).remove();
            }
            setTimeout(function () { window.scrollTo(0, 0); }, 1000);
        }
    }
}

//{ url, body, callBackFunction, uploadButtonContainer, msgBox}
function uploadFile(parameters) {
    if (parameters.uploadButtonContainer !== undefined) {
        $(`${parameters.uploadButtonContainer} > *`).toggle();
    }
    fetch(parameters.url, {
        method: 'POST',
        body: parameters.body,
        headers: { "X-CSRF-TOKEN-HEADERNAME": $("input[name='AntiforgeryFieldname']").val() }
    }).then(function (response) {
        if (response.status === 200) {
            response.json().then(function (apiResponse) {
                let res = apiResponse;
                $(`${parameters.uploadButtonContainer} > *`).toggle();
                if (parameters.callBackFunction !== undefined) {
                    parameters.callBackFunction(res);
                }
                if (parameters.msgBox !== undefined) {
                    $(parameters.msgBox).html(net3000.common.msgBox(res));
                }
            });
        }
    });
}

function logoutfun() {
    sessionStorage.removeItem("adminMenu");
    localStorage.removeItem("userView");
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    localStorage.removeItem("tokenexpiry");
    window.location.href = "/logout";
}

var customFieldTemplate = `
<div>
    <div class="form-group row" v-for="(pair,index) in metaObject">
        <div class="col-md-3">
            <div class="input-group">
                <input name="key" class="form-control" placeholder="Title" v-model="pair.key" v-on:change="updateObject()" />
                <div class="input-group-append">
                    <span class="input-group-text">
                        <a href="javascript:" class="text-dark" v-on:click="removeItem(index)"><i class="fa fa-trash"></i></a>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-md-9">
            <textarea rows="3" class="form-control" placeholder="Value" v-model="pair.value" v-on:change="updateObject()"></textarea>
        </div>
    </div>
    <p align='center' v-if='metaObject.length == 0'>There are no custom fields.</p>
    <div class='bg-light p-3 rounded border'>
        <div class="row">
            <div class="col-md-3">
                <div class="input-group">
                    <input name="key" class="form-control" placeholder="Title" v-model="key" />
                    <div class="input-group-append">
                        <span class="input-group-text">
                            <a href="javascript:" class="text-dark" v-on:click="addNewItem()"><i class="fa fa-plus mr-2"></i> Add</a>
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-md-9">
                <textarea rows="3" class="form-control" placeholder="Value" v-model="value"></textarea>
            </div>
        </div>
    </div>
</div>
`;

//parameters include id, data, action
Vue.component('customfields', {
    template: customFieldTemplate,
    props: ["metadata"],
    data: function () {
        return {
            propertyList: [],
            key: "",
            value: ""
        }
    },
    computed: {
        metaObject: function () {
            if (this.propertyList.length == 0) {
                if (this.metadata != undefined) {
                    for (prop in this.metadata) {
                        this.propertyList.push({ key: prop, value: this.metadata[prop] });
                    }
                }
            }
            return this.propertyList;
        }
    },
    methods: {
        reset: function () {
            this.propertyList = [];
        },
        addNewItem: function () {
            if (this.metadata == undefined) {
                this.metadata = {};
            }
            if (this.key == "") { return; }
            this.propertyList.push({ key: this.key, value: this.value });
            this.metadata[this.key] = this.value;
            this.key = "";
            this.value = "";
        },
        removeItem: function (index) {
            delete this.metadata[this.propertyList[index].key];
            this.propertyList.splice(index, 1);
        },
        updateObject: function () {
            if (this.metadata == undefined) {
                this.metadata = {};
            }
            for (prop of this.propertyList) {
                this.metadata[prop.key] = prop.value;
            }
            for (prop of this.metadata) {
                if (this.propertyList.find(p => p.key == prop).length == 0) {
                    delete this.metadata[prop];
                };
            }
        }
    }
});
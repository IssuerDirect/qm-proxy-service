var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var net3000;
(function (net3000) {
    class standardMessages {
        constructor() { }
        static get saved() { return new apiResponse(200, "Saved", "Your changes have been saved", "success", "fa-check-circle"); }
        static get loggedOut() { return new apiResponse(200, "Logged Out", "You're now logged out.", "success", "fa-check-circle"); }
        static get deleted() { return new apiResponse(200, "Deleted", "The record you selected has been deleted", "success", "fa-check-circle"); }
        static get invalid() { return new apiResponse(400, "Invalid Request", "Your request is invalid", "danger", "fa-times-circle"); }
        static get missingRequired() { return new apiResponse(400, "Missing Required Fields", "Fill the required fields to continue", "danger", "fa-times-circle"); }
        static get loggedIn() { return new apiResponse(200, "Logged In", "You are now logged in", "success", "fa-check-circle"); }
        static get invalidLogin() { return new apiResponse(401, "Invalid Login", "Your login is incorrect", "danger", "fa-times-circle", 0); }
        static get notFound() { return new apiResponse(404, "Not Found", "The record you were trying to reach was not found", "danger", "fa-times-circle", 0); }
        static get notActive() { return new apiResponse(401, "Not Active", "The account you were trying to access is not active", "danger", "fa-times-circle"); }
        static get found() { return new apiResponse(302, "Found", "Found the record you requested", "success", "fa-check-circle"); }
        static get accountNotFound() {
            return new apiResponse(404, "Account Not Found", "I can't find an account for this website", "danger", "fa-times-circle");
        }
    }
    net3000.standardMessages = standardMessages;
    class apiResponse {
        constructor(code = 200, title, message, cssClass = "success", icon = "fa-check-circle", count = 1) {
            this.pageIndex = 0;
            this.title = title;
            this.message = message;
            this.cssClass = cssClass;
            this.icon = icon;
            this.code = code;
            this.count = count;
        }
        html() {
            return `
                <div class="alert alert-${this.cssClass} alert-dismissible fade show" role="alert">
                    <i class="fa ${this.icon} mr-2"></i><strong>${this.title}</strong>. ${this.message}.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>`;
        }
    }
    net3000.apiResponse = apiResponse;
    class commonParameters {
        constructor() {
            this.append = false;
        }
    }
    net3000.commonParameters = commonParameters;
    class contact {
        constructor() {
            this.isNew = null;
            this.loggedIn = false;
        }
    }
    net3000.contact = contact;
    class address {
    }
    net3000.address = address;
    class order {
        constructor() {
            this.shippingContact = new address();
            this.billingContact = new address();
            this.currency = "CAD";
            this.discount = 0;
            this.shipping = 0;
            this.total = 0;
            this.subtotal = 0;
            this.tax = 0;
            this.orderItems = [];
            this.customer = new contact();
            this.orderItems = [];
            this.creditCard = new creditCard();
        }
    }
    net3000.order = order;
    class orderItem {
        constructor() {
            this.qty = 1;
        }
        get total() {
            if (this.price != null) {
                return this.price * this.qty;
            }
        }
    }
    net3000.orderItem = orderItem;
    class creditCard {
        constructor() { }
    }
    net3000.creditCard = creditCard;
    class loginUser {
        constructor() {
            this.id = 0;
            this.metaDictionary = {};
            this.loggedIn = false;
            this.isNew = null;
        }
        passwordMatches() {
            return this.password == this.confirmPassword;
        }
    }
    net3000.loginUser = loginUser;
    class account {
        constructor() { }
    }
    net3000.account = account;
    class common {
        static apiURL() {
            //if (net3000.common.apiurl != undefined) { return net3000.common.apiurl; }
            //if (window.location.host.includes("localhost")) {
            //    return "http://localhost:3000/";
            //} else {
            return "https://api.net3000.ca/v2/";
            //}
        }
        static cdnURL() {
            if (net3000.common.apiurl != undefined) {
                return net3000.common.apiurl;
            }
            if (this.testMode == true) {
                return "/";
            }
            else {
                return "https://cdn.net3000.ca/";
            }
        }
        static verifyCaptcha(parameters) {
            if (parameters.actionName == undefined) {
                parameters.actionName = "website";
            }
            grecaptcha.ready(function () {
                grecaptcha.execute('6Le1cs4UAAAAAFzAcc-6pcJDZI2A2hg0cEaDCGvT', { action: parameters.actionName }).then(function (token) {
                    let apiResponse = net3000.common.handlePromise({ url: `recaptcha?token=${token}`, parameters: { account: 0 } });
                    if (apiResponse != null) {
                        apiResponse.then(function (response) {
                            if (response.status == 200) {
                                response.json().then(function (myResponse) {
                                    if (Number(myResponse.data.score) > 0.5 && myResponse.data.success == true && myResponse.data.action == parameters.actionName) {
                                        parameters.callBackFunction();
                                    }
                                    else {
                                        console.info("low score" + Number(myResponse.data.score));
                                    }
                                });
                            }
                            else {
                                console.info(response);
                            }
                        });
                    }
                });
            });
        }
        static message(parameters = Object.assign({ messageid: "" }, commonParameters)) {
            if (parameters.messageid != undefined) {
                parameters.messageid = parameters.messageid.replace(" ", "+");
            }
            net3000.common.handlePromise({ url: `common/appmessage?messageid=${parameters.messageid}`, parameters: parameters });
        }
        addScript(paths) {
            var arr = paths.split(',');
            for (var i = 0; i < arr.length; i++) {
                var s = document.createElement("script");
                s.type = "text/javascript";
                s.src = arr[i];
                document.head.appendChild(s);
            }
        }
        //Validation functions
        static checkEmail(email) {
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
        }
        static getCookie(name) {
            if (this.cookie == null) {
                this.cookie = new Object();
                for (let value of document.cookie.split(';')) {
                    value = value.trim();
                    if (value.split("=")[0] != null) {
                        this.cookie[value.split("=")[0]] = value.split("=")[1];
                    }
                }
            }
            if (this.cookie[name] != undefined && this.cookie[name] != null) {
                return this.cookie[name];
            }
            return null;
        }
        static getQueryString(key, query = null) {
            if (common.queryDictionary == undefined) {
                common.queryDictionary = {};
                let search;
                if (query != undefined) {
                    search = query;
                }
                else {
                    search = window.location.search;
                }
                if (search != undefined) {
                    let parameters = search.replace("?", "").split('&');
                    for (let parameter of parameters) {
                        let keyvalue = parameter.split('=');
                        common.queryDictionary[keyvalue[0]] = keyvalue[1];
                    }
                }
            }
            if (common.queryDictionary[key] != undefined) {
                return common.queryDictionary[key];
            }
            return null;
        }
        static parameterAccount(account) {
            if (account == undefined || account == null) {
                if (this.getCookie("account") != null) {
                    if (this.getCookie("account") != null) {
                        account = parseInt(this.getCookie("account"));
                    }
                }
                else if (net3000.common.currentAccount != undefined) {
                    account = net3000.common.currentAccount.id;
                }
            }
            return account;
        }
        //sample for getting value in 1 line. This needs to run inside a: (async function) var apiResponse = await (await net3000.common.handlePromise({ url: 'getAccount', parameters: { account: 20}})).json();
        static handlePromise(myParameters) {
            if (myParameters.method == undefined) {
                if (myParameters.body != undefined || myParameters.formData != undefined) {
                    myParameters.method = "POST";
                }
                else {
                    myParameters.method = "GET";
                }
            }
            if (myParameters.parameters == undefined) {
                myParameters.parameters = {};
            }
            myParameters.parameters.account = net3000.common.parameterAccount(myParameters.parameters.account);
            if (myParameters.parameters.buttonContainer != undefined) {
                $(`${myParameters.parameters.buttonContainer} > *`).toggle();
            }
            let fetchUrl = net3000.common.apiURL() + myParameters.url;
            if (myParameters.apiurl != undefined) {
                fetchUrl = myParameters.apiurl;
            }
            let fetchBody;
            let fetchHeaders = myParameters.parameters.headers; //{ "Content-Type": contentType, ...myParameters.parameters.headers};
            if (fetchHeaders == undefined) {
                fetchHeaders = {};
            }
            if (myParameters.body != undefined) {
                fetchBody = myParameters.body;
                if (fetchHeaders["Content-Type"] == undefined) {
                    fetchHeaders["Content-Type"] = "application/json";
                }
            }
            if (myParameters.formData != undefined) {
                fetchBody = myParameters.formData;
            }
            if ($("input[name='AntiforgeryFieldname']").length > 0) {
                fetchHeaders["X-CSRF-TOKEN-HEADERNAME"] = $("input[name='AntiforgeryFieldname']").val();
            }
            //client tokens are saved in session storage, admin tokens are saved in local storage
            if (sessionStorage.getItem("token") != null) {
                fetchHeaders["Authorization"] = "Bearer " + sessionStorage.getItem("token");
            }
            else if (localStorage.getItem("token") != null) {
                fetchHeaders["Authorization"] = "Bearer " + localStorage.getItem("token");
            }
            if (myParameters.parameters.account != undefined) {
                fetchHeaders["account"] = myParameters.parameters.account;
            }
            let fetchFunction = fetch(fetchUrl, { method: myParameters.method, headers: fetchHeaders, body: fetchBody, credentials: "same-origin" });
            if (myParameters.parameters.container == undefined && myParameters.parameters.template == undefined && myParameters.parameters.callBackFunction == undefined && myParameters.parameters.msgBox == undefined && myParameters.parameters.modalMsgBox == undefined) {
                return fetchFunction;
            }
            fetchFunction.then(function (response) {
                if (response.status == 200) {
                    response.json().then(function (myResponse) {
                        let res = myResponse;
                        if (myParameters.parameters.container != undefined && myParameters.parameters.template != undefined) {
                            if (myParameters.parameters.append == true) {
                                $(myParameters.parameters.container).append(Mustache.render($(myParameters.parameters.template).html(), res));
                            }
                            else {
                                $(myParameters.parameters.container).html(Mustache.render($(myParameters.parameters.template).html(), res));
                            }
                        }
                        if (myParameters.parameters.loadMore != undefined) {
                            if (myResponse.pageSize != null && (myResponse.count <= (myResponse.pageSize * (myResponse.pageIndex + 1)) || myResponse.pageSize >= myResponse.count)) {
                                //hide load more
                                $(myParameters.parameters.loadMore).hide();
                                //if count > page size, show scroll to top
                                if (myParameters.parameters.scrollToTop != undefined && myResponse.count <= myResponse.pageSize * (myResponse.pageIndex + 1)) {
                                    $(myParameters.parameters.scrollToTop).removeClass("d-none");
                                    $(myParameters.parameters.scrollToTop).show();
                                }
                            }
                            else {
                                $(myParameters.parameters.loadMore).show();
                            }
                        }
                        if (myParameters.parameters.callBackFunction != undefined) {
                            myParameters.parameters.callBackFunction(res);
                        }
                        bindResponse(res);
                    });
                }
                else {
                    //If an error happens, and if there's a msg box, show message. If the button was toggled to disapear, show it.
                    let myResponse = net3000.standardMessages.invalid;
                    myResponse.title = "Error";
                    myResponse.message = `${response.status}: ${response.statusText}`;
                    bindResponse(myResponse);
                }
            });
            function bindResponse(res) {
                if (myParameters.parameters.modalMsgBox != undefined) {
                    $(myParameters.parameters.modalMsgBox).html(net3000.common.msgBox(res));
                }
                else if (myParameters.parameters.msgBox != undefined) {
                    $(myParameters.parameters.msgBox).html(net3000.common.msgBox(res));
                }
                if (myParameters.parameters.buttonContainer != undefined) {
                    $(`${myParameters.parameters.buttonContainer} > *`).toggle();
                }
            }
            return;
        }
        static handleEnter(parameters) {
            if (parameters.event.keyCode === 13) {
                parameters.event.preventDefault();
                parameters.event.stopPropagation();
                parameters.callBackFunction();
            }
        }
        static recaptcha(parameters) {
            net3000.common.handlePromise({ url: `recaptcha?token=${parameters.token}`, parameters: { callBackFunction: recapchaResponse } });
            function recapchaResponse(apiResponse) {
                if (Number(apiResponse.data.score) > 0.7 && apiResponse.data.action == parameters.action) {
                    parameters.callBackFunction();
                }
                else {
                    console.info(`Recaptcha score is ${apiResponse.data.score}`);
                }
            }
        }
        static collectData(selector, reset = true) {
            if (reset) {
                this.myData = {};
            }
            let myLocalData = this.myData;
            $(`${selector} :input`).each(function () {
                if ($(this).attr("type") == "checkbox" || $(this).attr("type") == "radio") {
                    return;
                }
                if ($(this).attr("name") != undefined) {
                    myLocalData[$(this).attr("name")] = $(this).val();
                }
            });
            $(`${selector} input[type='checkbox'],${selector} input[type='radio']`).each(function () {
                var collectUnchecked = true;
                if ($(`${selector} input[name='${$(this).attr("name")}']`).length > 1) {
                    collectUnchecked = false;
                }
                if ($(this).attr("name") != undefined) {
                    if ($(this).prop("checked")) {
                        if ($(this).attr("value") != undefined) {
                            if (myLocalData[$(this).attr("name")] == undefined) {
                                myLocalData[$(this).attr("name")] = $(this).attr("value");
                            }
                            else {
                                myLocalData[$(this).attr("name")] += ',' + $(this).attr("value");
                            }
                        }
                        else {
                            if (myLocalData[$(this).attr("name")] == undefined) {
                                myLocalData[$(this).attr("name")] = "true";
                            }
                            else {
                                myLocalData[$(this).attr("name")] += ',true';
                            }
                        }
                    }
                    else {
                        if (collectUnchecked) {
                            if (myLocalData[$(this).attr("name")] == undefined) {
                                myLocalData[$(this).attr("name")] = "false";
                            }
                            else {
                                myLocalData[$(this).attr("name")] += ',false';
                            }
                        }
                    }
                }
            });
            this.myData = myLocalData;
            return this.myData;
        }
        static collectDataDictionary(selector, reset = true) {
            if (reset) {
                this.myDictionary = [];
            }
            let myLocalDictionary = this.myDictionary;
            $(`${selector} :input`).each(function () {
                if ($(this).attr("name") !== undefined) {
                    let newPair = {};
                    newPair[$(this).attr("name")] = $(this).val();
                    myLocalDictionary.push(newPair);
                }
            });
            this.myDictionary = myLocalDictionary;
            return this.myDictionary;
        }
        static clearForm(selector) {
            $(`${selector} :input`).each(function () {
                $(this).val("");
            });
        }
        static getAccount(account) {
            account = this.parameterAccount(account);
            let accountStr = localStorage.getItem("account");
            if (accountStr == null) {
                this.loadAccountFromDB(account);
            }
            else {
                net3000.common.currentAccount = JSON.parse(accountStr);
                if (account != null && net3000.common.currentAccount.id !== account) {
                    this.loadAccountFromDB(account);
                }
                else if (net3000.common.currentAccount.lastUpdate == undefined || net3000.common.currentAccount.lastUpdate < (Date.now() - 86400)) {
                    this.loadAccountFromDB();
                }
            }
        }
        static loadAccountFromDB(account) {
            account = net3000.common.parameterAccount(account);
            let parameters = account == undefined ? "" : `?id=${account}`;
            fetch(`${net3000.common.apiURL()}getaccount${parameters}`).then(function (response) {
                if (response.status == 200) {
                    response.json().then(function (myResponse) {
                        if (myResponse.code == 302) {
                            net3000.common.currentAccount = myResponse.data;
                            net3000.common.currentAccount.lastUpdate = Date.now();
                            localStorage.setItem("account", JSON.stringify(net3000.common.currentAccount));
                            document.cookie = `account=${net3000.common.currentAccount.id};path=/;`;
                        }
                    });
                }
            });
        }
        static bindData(selector, myData) {
            $(`${selector} :input`).each(function () {
                $(this).val(myData[$(this).attr("name")]);
            });
        }
        static msgBox(myMsg) {
            return `
                <div class="alert alert-${myMsg.cssClass} alert-dismissible fade show" role="alert">
                    <i class="fa ${myMsg.icon} mr-2"></i><strong>${myMsg.title}</strong>. ${myMsg.message}.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>`;
        }
        static secureMe() {
            if (!window.location.href.startsWith("https://") && window.location.href.indexOf("localhost") < 0) {
                window.location.href = window.location.href.replace("http://", "https://");
            }
        }
        static switchView(viewName) {
            if (net3000.common.currentView != undefined) {
                $(net3000.common.currentView).hide();
            }
            net3000.common.currentView = viewName;
            $(viewName).show();
        }
        static pickRandomImage(urls, backgroundSelector = "background-image") {
            var StaticImgURL = urls[Math.floor(Math.random() * urls.length)];
            if (backgroundSelector != undefined && $(backgroundSelector).length > 0) {
                $(backgroundSelector).css("background-image", "url(" + StaticImgURL + ")");
            }
            return StaticImgURL;
        }
        static getUser() {
            return __awaiter(this, void 0, void 0, function* () {
                if (localStorage.getItem("token") == undefined && sessionStorage.getItem("token") == undefined) {
                    return null;
                }
                let res = yield (yield net3000.common.handlePromise({
                    url: "user"
                })).json();
                if (res.code == 200) {
                    let myContact = new loginUser();
                    myContact.firstName = res.data.firstName;
                    myContact.lastName = res.data.lastName;
                    myContact.email = res.data.email;
                    myContact.phone = res.data.phone;
                    myContact.loggedIn = true;
                    myContact.isNew = false;
                    myContact.loginGroupID = res.data.loginGroupID;
                    if (res.data.loginGroup != undefined) {
                        myContact.mainPage = res.data.loginGroup.mainPage;
                    }
                    if (res.data.metaDataDictionary == undefined) {
                        myContact.metaDictionary = {};
                    }
                    else {
                        for (let key in res.data.metaDataDictionary) {
                            myContact[key] = res.data.metaDataDictionary[key];
                        }
                    }
                    return myContact;
                }
                else {
                    sessionStorage.removeItem("token");
                    sessionStorage.removeItem("user");
                    localStorage.removeItem("token");
                    localStorage.removeItem("user");
                }
                return null;
            });
        }
        static getAddresses() {
            return __awaiter(this, void 0, void 0, function* () {
                if (localStorage.getItem("token") == undefined) {
                    return null;
                }
                let res = yield (yield net3000.common.handlePromise({
                    url: "addresses"
                })).json();
                if (res.code == 302) {
                    return res.data;
                }
                return null;
            });
        }
        static logOut(redirect) {
            localStorage.removeItem("user");
            localStorage.removeItem("token");
            if (redirect != undefined) {
                window.location.href = redirect;
            }
        }
        static initializeLogin() {
            return __awaiter(this, void 0, void 0, function* () {
                let loginTemplate = yield (yield net3000.common.handlePromise({ url: "common/setting/loginComponent" })).json();
                Vue.component('useraccount', {
                    template: loginTemplate.data,
                    props: ['logingroup', 'client'],
                    data: function () {
                        return {
                            showAuhorization: false,
                            currentView: 'login',
                            msgBox: '',
                            processing: false
                        };
                    },
                    methods: {
                        checkEmail: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                let form = document.querySelector("#loginForm input[name=email]");
                                if (!form.checkValidity()) {
                                    this.msgBox = net3000.common.msgBox(net3000.standardMessages.invalid);
                                    return;
                                }
                                let response = yield (yield net3000.common.handlePromise({ url: `checkEmail?email=${this.client.email}&loginGroup=${this.logingroup}` })).json();
                                if (response.code == 302) {
                                    this.client.isNew = false;
                                }
                                else {
                                    this.client.isNew = true;
                                }
                            });
                        },
                        getAuthorizationCode: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                this.processing = true;
                                let response = yield (yield net3000.common.handlePromise({ url: `sendAuthorizationCode?email=${this.client.email}&loginGroup=${this.logingroup}` })).json();
                                if (response.code == 302) {
                                    this.msgBox = '';
                                    this.showAuhorization = true;
                                }
                                this.processing = false;
                            });
                        },
                        verifyCode: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                this.processing = true;
                                let response = yield (yield net3000.common.handlePromise({ url: `authorizeCode?email=${this.client.email}&code=${this.client.authorizationCode}` })).json();
                                debugger;
                                if (response.code == 200) {
                                    this.client.firstName = response.data.client.firstName;
                                    this.client.lastName = response.data.client.lastName;
                                    this.client.email = response.data.client.email;
                                    this.client.isNew = false;
                                    this.client.loggedIn = true;
                                    this.client.loginGroupID = response.data.client.loginGroupID;
                                    this.client.mainPage = response.data.client.mainPage;
                                    localStorage.setItem("token", response.data.token);
                                    localStorage.setItem(response.data.client.loginGroupID, JSON.stringify(response.data.client));
                                    this.msgBox = '';
                                }
                                else {
                                    this.showAuhorization = null;
                                    this.msgBox = response.html;
                                }
                                this.processing = false;
                            });
                        },
                        login: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                this.processing = true;
                                let response = yield (yield net3000.common.handlePromise({
                                    url: "login", body: JSON.stringify({ "email": this.client.email, "password": this.client.password, "loginGroupID": this.logingroup })
                                })).json();
                                if (response.code == 200) {
                                    this.client.loggedIn = true;
                                    this.client.firstName = response.data.client.firstName;
                                    this.client.lastName = response.data.client.lastName;
                                    this.client.email = response.data.client.email;
                                    this.client.loginGroupID = response.data.client.loginGroupID;
                                    this.client.mainpage = response.data.client.mainpage;
                                    localStorage.setItem("token", response.data.token);
                                    localStorage.setItem("user", JSON.stringify(this.client));
                                    this.msgBox = '';
                                    this.$emit("continue");
                                }
                                else {
                                    //$("#userLoginButtonContainer > *").toggle();
                                    this.processing = false;
                                    this.showAuhorization = null;
                                    this.msgBox = response.html;
                                }
                            });
                        },
                        register: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                //$("#userRegisterButtonContainer > *").toggle();
                                this.processing = true;
                                let response = yield (yield net3000.common.handlePromise({
                                    url: "registerOrGet", body: JSON.stringify({
                                        firstName: this.client.firstName,
                                        lastName: this.client.lastName,
                                        email: this.client.email,
                                        loginGroupID: this.logingroup,
                                        password: this.client.password
                                    })
                                })).json();
                                if (response.code == 200) {
                                    this.client.loggedIn = true;
                                    this.client.firstName = response.data.client.firstName;
                                    this.client.lastName = response.data.client.lastName;
                                    this.client.email = response.data.client.email;
                                    this.client.loginGroupID = response.data.client.loginGroupID;
                                    this.client.mainpage = response.data.client.mainpage;
                                    localStorage.setItem("token", response.data.token);
                                    localStorage.setItem("user", JSON.stringify(this.client));
                                    this.msgBox = '';
                                    this.$emit("continue");
                                }
                                else {
                                    //$("#userRegisterButtonContainer > *").toggle();
                                    this.showAuhorization = null;
                                    this.msgBox = response.html;
                                }
                                this.processing = false;
                            });
                        },
                        logout: function () {
                            if (localStorage.getItem("token") != undefined) {
                                localStorage.removeItem("token");
                            }
                            if (localStorage.getItem("user") != undefined) {
                                localStorage.removeItem("user");
                            }
                            this.client.loggedIn = false;
                            this.client.firstName = null;
                            this.client.lastName = null;
                            this.client.email = null;
                            this.client.loginGroupID = this.logingroup;
                        }
                    },
                    mounted: function () {
                        return __awaiter(this, void 0, void 0, function* () {
                            let user = yield net3000.common.getUser();
                            if (user == null) {
                                this.client.loggedIn = false;
                            }
                            else {
                                this.client.loggedIn = true;
                                this.client.isNew = false;
                                this.client.firstName = user.firstName;
                                this.client.lastName = user.lastName;
                                this.client.email = user.email;
                                this.client.loginGroupID = user.loginGroupID;
                                this.client.password = undefined;
                            }
                        });
                    }
                });
            });
        }
        static initializeAccountUpdate() {
            return __awaiter(this, void 0, void 0, function* () {
                let loginTemplate = yield (yield net3000.common.handlePromise({ url: "common/setting/updateAccount" })).json();
                Vue.component('userupdate', {
                    template: loginTemplate.data,
                    props: ['client'],
                    data: function () {
                        return {
                            msgBox: '',
                            processing: false
                        };
                    },
                    methods: {
                        validateCustomField: function () {
                            let confirmBox = document.getElementById("confirmPassword");
                            if (this.client.password == undefined || this.client.password == '' || this.client.password == this.client.confirmPassword) {
                                confirmBox.setCustomValidity('');
                            }
                            else {
                                confirmBox.setCustomValidity("Password not matching");
                            }
                        },
                        updateAccount: function () {
                            return __awaiter(this, void 0, void 0, function* () {
                                this.processing = true;
                                this.msgBox = {};
                                let AddressForm = document.querySelector("form.clientDetails");
                                if (!AddressForm.checkValidity() || !(this.client.password == undefined || this.client.password == '' || this.client.password == this.client.confirmPassword)) {
                                    $("form.clientDetails").addClass("was-validated");
                                    this.processing = false;
                                    return;
                                }
                                else {
                                    $("form.clientDetails").removeClass("was-validated");
                                }
                                let postClient = Object.assign({}, this.client);
                                delete postClient.isNew;
                                delete postClient.loggedIn;
                                delete postClient.id;
                                delete postClient.metaDictionary;
                                delete postClient.mainPage;
                                delete postClient.loginGroupID;
                                let res = yield (yield (net3000.common.handlePromise({ url: "user", body: JSON.stringify(postClient), method: "put" }))).json();
                                this.msgBox = res;
                                this.processing = false;
                            });
                        },
                        logout: function () {
                            if (localStorage.getItem("token") != undefined) {
                                localStorage.removeItem("token");
                            }
                            if (localStorage.getItem("user") != undefined) {
                                localStorage.removeItem("user");
                            }
                            this.client.isNew = null;
                            this.client.loggedIn = false;
                            this.client.firstName = null;
                            this.client.lastName = null;
                            this.client.email = null;
                        }
                    }
                });
            });
        }
        static initializeProvinceList() {
            return __awaiter(this, void 0, void 0, function* () {
                //let loginTemplate: apiResponse = await (await net3000.common.handlePromise({ url: "common/setting/provinceList" })).json();
                let allStatesAndProvinces = yield (yield net3000.common.handlePromise({ url: "/provinces" })).json();
                Vue.component('provinces', {
                    template: `<div>
        <select class="form-control" v-if="myProvinces.length > 0" v-model="address.provinceid">
            <option value="">Select {{provinceTitle}}</option>
            <option v-for="province of myProvinces" :value="province.id">{{province.province}}</option>
        </select>
        <input type="text" class="form-control" v-if="myProvinces.length == 0" v-model="address.province"/>
    </div>`,
                    props: ['countryid', 'address'],
                    data: function () {
                        return {
                            provinces: allStatesAndProvinces
                        };
                    },
                    computed: {
                        myProvinces: function () {
                            return this.provinces.filter((p) => p.countryID == this.countryid);
                        },
                        provinceTitle: function () {
                            if (this.countryid == "US") {
                                return "State";
                            }
                            if (this.countryid == "CA") {
                                return "Province";
                            }
                            return "Region";
                        }
                    }
                });
            });
        }
        static getData(parameters = { elementID: null, source: null, localStorage: null, sessionStorage: null }) {
            return __awaiter(this, void 0, void 0, function* () {
                if (parameters.elementID == undefined || parameters.source == undefined) {
                    return;
                }
                let dataSource = standardMessages.found;
                let found = false;
                if (parameters.localStorage != undefined) {
                    if (localStorage.getItem(parameters.localStorage) != undefined) {
                        dataSource = JSON.parse(localStorage.getItem(parameters.localStorage));
                        found = true;
                    }
                }
                else if (parameters.sessionStorage != undefined) {
                    if (sessionStorage.getItem(parameters.sessionStorage) != undefined) {
                        dataSource = JSON.parse(sessionStorage.getItem(parameters.sessionStorage));
                        found = true;
                    }
                }
                if (!found) {
                    dataSource = yield (yield net3000.common.handlePromise({ url: parameters.source })).json();
                    if (parameters.localStorage != undefined) {
                        localStorage.setItem(parameters.localStorage, JSON.stringify(dataSource));
                    }
                    if (parameters.sessionStorage != undefined) {
                        sessionStorage.setItem(parameters.sessionStorage, JSON.stringify(dataSource));
                    }
                }
                net3000.common.apps[parameters.elementID] = new Vue({
                    el: "#" + parameters.elementID,
                    data: dataSource
                });
            });
        }
    }
    //Generated from type
    //Global Variables 
    common.apps = {};
    common.testMode = false;
    common.cookie = null;
    common.myData = {};
    common.myDictionary = [];
    common.currentView = undefined;
    //Global Variables
    common.currentAccount = new net3000.account();
    common.myResponse = new net3000.apiResponse();
    net3000.common = common;
})(net3000 || (net3000 = {}));
$(function () {
    $("[data-getdata]").each(function () {
        net3000.common.getData({ elementID: $(this).attr("id"), source: $(this).data("getdata"), localStorage: $(this).data("localstorage"), sessionStorage: $(this).data("sessionstorage") });
    });
});
//# sourceMappingURL=common.js.map
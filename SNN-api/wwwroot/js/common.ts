namespace net3000 {

    export class standardMessages {
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

    export class apiResponse {
        constructor(code: number = 200, title?: string, message?: string, cssClass: string = "success", icon: string = "fa-check-circle", count: number = 1) {
            this.title = title;
            this.message = message;
            this.cssClass = cssClass;
            this.icon = icon;
            this.code = code;
            this.count = count;
        }
        pageIndex: number = 0;
        pageSize?: number;
        code: number;
        title?: string;
        message?: string;
        cssClass: string;
        icon: string;
        data: any;
        count: number;
        get html(): string {
            return `
                <div class="alert alert-${this.cssClass} alert-dismissible fade show" role="alert">
                    <i class="fa ${this.icon} mr-2"></i><strong>${this.title}</strong>. ${this.message}.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>`;
        }
    }
    
    export class commonParameters {
        account?: number
        template?: string
        container?: string
        callBackFunction?: Function
        msgBox?: string
        modalMsgBox?: string
        buttonContainer?: string
        headers?: any
        append: boolean = false
        loadMore?: string
    }

    export class contact {
        constructor() { }
        email?: string;
        firstName?: string;
        lastName?: string;
        phone?: string;
        password?: string;
        isNew?: boolean = null;
        loggedIn: boolean = false;
        loginGroupID: string;
        mainPage: string;
    }

    export class address {
        firstName?: string;
        lastName?: string;
        phone?: string;
        email?: string;
        address?: string;
        address2?: string;
        city?: string;
        province?: string;
        provinceID?: string;
        postalCode?: string;
        countryID?: string;
        company?: string;
    }

    export class order {
        constructor() {
            this.customer = new contact();
            this.orderItems = [];
            this.creditCard = new creditCard();
        }
        shippingContact: address = new address();
        billingContact: address = new address();
        currency: string = "CAD";
        creditCard?: creditCard;
        customer?: contact;
        invoice?: string;
        poNumber?: string;
        description?: string;
        promoCode?: string;
        discount: number = 0;
        shipping: number = 0;
        total: number =0;
        subtotal: number =0;
        tax: number =0;
        orderItems: orderItem[] = [];
        thankYouUrl?: string;
        notifyUrl?: string;
        cancelUrl?: string;
        clientRequirements?: string;
    }

    export class orderItem {
        id?: number;
        sku?: string;
        featureImage?: string;
        title?: string;
        price?: number;
        qty: number = 1;
        unitPrice?: number;
        tax?: number;
        discount?: number;
        shipping?: number;
        originalPrice?: number;
        get total():number | undefined {
            if (this.price != null) {
                return this.price * this.qty;
            }
        }
    }

    export class creditCard {
        constructor() { }
        cardnumber?: string;
        expiryMonth?: number;
        expiryYear?: number;
        cvc?: number;
    }

    export class loginUser {
        id: number = 0;
        email: string;
        password: string;
        confirmPassword: string;
        firstName: string;
        lastName: string;
        loginGroupID: string;
        phone: string;
        mainPage: string;
        metaDictionary: any = {};
        loggedIn: boolean = false;
        isNew?: boolean = null;
        passwordMatches():boolean {
            return this.password == this.confirmPassword;
        }
    }

    export class account {
        constructor() { }
        id?: number;
        company?: string;
        email?: string;
        provinceid?: string;
        countryid?: string;
        currencysymbol?: string;
        accountSettings?: { smtp_host: string, smtp_username: string, smtp_password: string, smtp_port?: Number, DefaultLanguage: string, Currency: string };
        lastUpdate?: number;
    }

    export class common {

        //Generated from type
        //Global Variables 
        static apps: any = {};
        static testMode: boolean = false;
        static apiurl: string;
        static apiURL(): string {
            if (net3000.common.apiurl != undefined) { return net3000.common.apiurl; }
            return "https://api.net3000.ca/v2/";
        }

        static cdnURL(): string {
            if (net3000.common.apiurl != undefined) { return net3000.common.apiurl; }
            if (this.testMode == true) {
                return "/";
            } else {
                return "https://cdn.net3000.ca/";
            }
        }

        static loginLink(parameters: { email: string, callBackFunction: Function }) {
            net3000.common.handlePromise({ url: `loginLink?email=${parameters.email}`, parameters: { callBackFunction: parameters.callBackFunction } });
        }

        static verifyLoginLink(parameters: { token: string, callBackFunction: Function }) {
            net3000.common.handlePromise({ url: `authorizeLoginLink?token=${parameters.token}`, parameters: { callBackFunction: parameters.callBackFunction } });
        }

        static verifyCaptcha(parameters: { callBackFunction: Function, actionName: string, sitekey: string }): void {
            if (parameters.actionName == undefined) { parameters.actionName = "website"; }
            if (parameters.sitekey == undefined) { parameters.sitekey = '6Le1cs4UAAAAAFzAcc-6pcJDZI2A2hg0cEaDCGvT';}
            grecaptcha.ready(function () {
                grecaptcha.execute(parameters.sitekey, { action: parameters.actionName }).then(function (token) {
                    let apiResponse = net3000.common.handlePromise({ url: `recaptcha?token=${token}`, parameters: { account: 0 } });
                    if (apiResponse != null) {
                        apiResponse.then(function (response) {
                            if (response.status == 200) {
                                response.json().then(function (myResponse: apiResponse) {
                                    if (Number(myResponse.data.score) > 0.5 && myResponse.data.success == true && myResponse.data.action == parameters.actionName) {
                                        parameters.callBackFunction();
                                    } else {
                                        console.info("low score" + Number(myResponse.data.score));
                                    }
                                });
                            } else {
                                console.info(response);
                            }
                        });
                    }
                });
            });            
        }
        static message(parameters: any = { messageid: "", ...commonParameters }): void {
            if (parameters.messageid != undefined) { parameters.messageid = parameters.messageid.replace(" ","+");}
            net3000.common.handlePromise({ url: `common/appmessage?messageid=${parameters.messageid}`, parameters: parameters});
        }
        addScript(paths: string) {
            var arr = paths.split(',');
            for (var i = 0; i < arr.length; i++) {
                var s = document.createElement("script");
                s.type = "text/javascript";
                s.src = arr[i];
                document.head.appendChild(s);
            }
        }

        //Validation functions
        static checkEmail(email: string): boolean {
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
        }

        static cookie: any = null;

        static getCookie(name: string): string|null {
            if (this.cookie == null) {
                this.cookie = new Object();
                for (let value of document.cookie.split(';')) {
                    value = value.trim();
                    if (value.split("=")[0] != null) {
                        this.cookie[value.split("=")[0]] = value.split("=")[1];
                    }
                }
            }
            if (this.cookie[name] != undefined && this.cookie[name] != null) { return this.cookie[name]; }
            return null;
        }

        static queryDictionary: any;
        static getQueryString(key: string, query: string = null): string|null {
            if (common.queryDictionary == undefined) {
                common.queryDictionary = {};
                let search: string;
                if (query != undefined) {
                    search = query;
                } else {
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
            if (common.queryDictionary[key] != undefined) { return common.queryDictionary[key]; }
            return null;
        }

        static parameterAccount(account?: number): number | undefined {
            if (account == undefined || account == null) {
                if (net3000Account != undefined) {
                    account = net3000Account;
                }
               else if (this.getCookie("account") != null) {
                    if (this.getCookie("account") != null) {
                        account = parseInt(this.getCookie("account") as string);
                    }
                } else if (net3000.common.currentAccount != undefined) {
                    account = net3000.common.currentAccount.id;
                }
            }
            return account;
        }
        //sample for getting value in 1 line. This needs to run inside a: (async function) var apiResponse = await (await net3000.common.handlePromise({ url: 'getAccount', parameters: { account: 20}})).json();
        static handlePromise(myParameters: { url?: string, parameters?: any, method?: string, body?: string, apiurl?: string, formData?:FormData }) {
            if (myParameters.method == undefined) {
                if (myParameters.body != undefined || myParameters.formData != undefined) {
                    myParameters.method = "POST";
                } else {
                    myParameters.method = "GET";
                }
            }

            if (myParameters.parameters == undefined) {
                myParameters.parameters = {};}

            myParameters.parameters.account = net3000.common.parameterAccount(myParameters.parameters.account);
            if (myParameters.parameters.buttonContainer != undefined) {
                jQuery(`${myParameters.parameters.buttonContainer} > *`).toggle();
            }

            let fetchUrl = net3000.common.apiURL() + myParameters.url;
            if (myParameters.apiurl != undefined) {
                fetchUrl = myParameters.apiurl;
            }
            
            let fetchBody: any;
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
            if (myParameters.formData != undefined) {fetchBody = myParameters.formData;}            
            if (jQuery("input[name='AntiforgeryFieldname']").length > 0) {
                fetchHeaders["X-CSRF-TOKEN-HEADERNAME"] = jQuery("input[name='AntiforgeryFieldname']").val() as string;
            }
            //client tokens are saved in session storage, admin tokens are saved in local storage
            if (sessionStorage.getItem("token") != null) {
                fetchHeaders["Authorization"] = "Bearer " + sessionStorage.getItem("token");
            } else if (localStorage.getItem("token") != null) {
                fetchHeaders["Authorization"] = "Bearer " + localStorage.getItem("token");
            }
            if (myParameters.parameters.account != undefined) {
                fetchHeaders["account"] = myParameters.parameters.account;
            }      
            let fetchFunction = fetch(fetchUrl, { method: myParameters.method, headers: fetchHeaders, body: fetchBody, credentials: "same-origin"});
            if (myParameters.parameters.container == undefined && myParameters.parameters.template == undefined && myParameters.parameters.callBackFunction == undefined && myParameters.parameters.msgBox == undefined && myParameters.parameters.modalMsgBox == undefined) {
                return fetchFunction;
            }
            fetchFunction.then(function (response) {
                if (response.status == 200) {
                    response.json().then(function (myResponse: apiResponse) {
                        let res = myResponse;
                        if (myParameters.parameters.container != undefined && myParameters.parameters.template != undefined) {
                            if (myParameters.parameters.append == true) {
                                jQuery(myParameters.parameters.container).append(Mustache.render(jQuery(myParameters.parameters.template).html(), res));
                            } else {
                                jQuery(myParameters.parameters.container).html(Mustache.render(jQuery(myParameters.parameters.template).html(), res));
                            }
                        }

                        if (myParameters.parameters.loadMore != undefined) {                             
                            if (myResponse.pageSize != null && (myResponse.count <= (myResponse.pageSize * (myResponse.pageIndex + 1)) || myResponse.pageSize >= myResponse.count)) {
                                //hide load more
                                jQuery(myParameters.parameters.loadMore).hide();
                                //if count > page size, show scroll to top
                                if (myParameters.parameters.scrollToTop != undefined && myResponse.count <= myResponse.pageSize * (myResponse.pageIndex + 1)) {
                                    jQuery(myParameters.parameters.scrollToTop).removeClass("d-none");
                                    jQuery(myParameters.parameters.scrollToTop).show();
                                }
                            } else {
                                jQuery(myParameters.parameters.loadMore).show();
                            }
                        }

                        if (myParameters.parameters.callBackFunction != undefined) {
                            myParameters.parameters.callBackFunction(res);
                        }
                        bindResponse(res);
                    });
                } else {
                    //If an error happens, and if there's a msg box, show message. If the button was toggled to disapear, show it.
                    let myResponse: apiResponse = net3000.standardMessages.invalid;
                    myResponse.title = "Error";
                    myResponse.message = `${response.status}: ${response.statusText}`;
                    bindResponse(myResponse);
                }
            });

            function bindResponse(res: apiResponse): void {
                if (myParameters.parameters.modalMsgBox != undefined) {
                    jQuery(myParameters.parameters.modalMsgBox).html(net3000.common.msgBox(res));
                } else if (myParameters.parameters.msgBox != undefined) {
                    jQuery(myParameters.parameters.msgBox).html(net3000.common.msgBox(res));
                }
                if (myParameters.parameters.buttonContainer != undefined) {
                    jQuery(`${myParameters.parameters.buttonContainer} > *`).toggle();
                }
            }
            return;
        }

        static handleEnter(parameters: { event: any, callBackFunction: Function }): void {
            if (parameters.event.keyCode === 13) {
                parameters.event.preventDefault();
                parameters.event.stopPropagation();
                parameters.callBackFunction();
            }
        }

        static async recaptcha(parameters: { token: string, action?: "website", callBackFunction: Function }) {
            if (parameters.callBackFunction == undefined) {
                let res = await (await net3000.common.handlePromise({ url: `recaptcha?token=${parameters.token}` })).json();
                if (res.data.score != undefined && Number(res.data.score) > 0.7) {
                    return true;
                }
                return false;
            }
            net3000.common.handlePromise({ url: `recaptcha?token=${parameters.token}`, parameters: { callBackFunction: recapchaResponse } });
            function recapchaResponse(apiResponse: apiResponse): void {
                if (Number(apiResponse.data.score) > 0.7 && apiResponse.data.action == parameters.action) {
                    parameters.callBackFunction();
                } else {
                    console.info(`Recaptcha score is ${apiResponse.data.score}`);
                }
            }
        }

        static myData: any = {};
        static collectData(selector: string, reset: boolean = true) {
            if (reset) { this.myData = {}; }
            let myLocalData = this.myData;
            jQuery(`${selector} :input`).each(function () {
                if (jQuery(this).attr("type") == "checkbox" || jQuery(this).attr("type") == "radio") { return; }
                if (jQuery(this).attr("name") != undefined) {
                    myLocalData[jQuery(this).attr("name") as string] = jQuery(this).val();
                }
            });            
            jQuery(`${selector} input[type='checkbox'],${selector} input[type='radio']`).each(function () {
                var collectUnchecked = true;
                if (jQuery(`${selector} input[name='${jQuery(this).attr("name")}']`).length > 1) {
                    collectUnchecked = false;
                }
                if (jQuery(this).attr("name") != undefined) {
                    if (jQuery(this).prop("checked")) {
                        if (jQuery(this).attr("value") != undefined) {
                            if (myLocalData[jQuery(this).attr("name") as string] == undefined) {
                                myLocalData[jQuery(this).attr("name") as string] = jQuery(this).attr("value");
                            } else {
                                myLocalData[jQuery(this).attr("name") as string] += ',' + jQuery(this).attr("value");
                            }
                        } else {
                            if (myLocalData[jQuery(this).attr("name") as string] == undefined) {
                                myLocalData[jQuery(this).attr("name") as string] = "true";
                            } else {
                                myLocalData[jQuery(this).attr("name") as string] += ',true';
                            }
                        }
                    } else {
                        if (collectUnchecked) {
                            if (myLocalData[jQuery(this).attr("name") as string] == undefined) {
                                myLocalData[jQuery(this).attr("name") as string] = "false";
                            } else {
                                myLocalData[jQuery(this).attr("name") as string] += ',false';
                            }
                        }
                    }
                }
            });
            this.myData = myLocalData;
            return this.myData;
        }

        static myDictionary: any = [];
        static collectDataDictionary(selector: string, reset: boolean = true) {
            if (reset) { this.myDictionary = []; }
            let myLocalDictionary = this.myDictionary;
            jQuery(`${selector} :input`).each(function () {
                if (jQuery(this).attr("name") !== undefined) {
                    let newPair: any = {};
                    newPair[jQuery(this).attr("name") as string] = jQuery(this).val();
                    myLocalDictionary.push(newPair);
                }
            });
            this.myDictionary = myLocalDictionary;
            return this.myDictionary;
        }

        static clearForm(selector: string) {
            jQuery(`${selector} :input`).each(function () {
                jQuery(this).val("");
            });
        }

        static getAccount(account?: number): void {
            account = this.parameterAccount(account);
            let accountStr = localStorage.getItem("account");
            if (accountStr == null) {
                this.loadAccountFromDB(account);
            } else {
                net3000.common.currentAccount = JSON.parse(accountStr);
                if (account != null && net3000.common.currentAccount.id !== account) {
                    this.loadAccountFromDB(account);
                } else if (net3000.common.currentAccount.lastUpdate == undefined || net3000.common.currentAccount.lastUpdate < (Date.now() - 86400)) {
                    this.loadAccountFromDB();
                }
            }
        }

        static loadAccountFromDB(account?: number) {
            account = net3000.common.parameterAccount(account);
            let parameters: string = account == undefined ? "" : `?id=${account}`;
            fetch(`${net3000.common.apiURL()}getaccount${parameters}`).then(function (response) {
                if (response.status == 200) {
                    response.json().then(function (myResponse: apiResponse) {
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

        static bindData(selector: string, myData: any) {
            jQuery(`${selector} :input`).each(function () {
                jQuery(this).val(myData[jQuery(this).attr("name") as string]);
            });
        }

        static msgBox(myMsg: apiResponse): string {
            return `
                <div class="alert alert-${myMsg.cssClass} alert-dismissible fade show" role="alert">
                    <i class="fa ${myMsg.icon} mr-2"></i><strong>${myMsg.title}</strong>. ${myMsg.message}.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>`;
        }

        static secureMe(): void {
            if (!window.location.href.startsWith("https://") && window.location.href.indexOf("localhost") < 0) {
                window.location.href = window.location.href.replace("http://", "https://");
            }
        }

        static currentView: string | undefined = undefined;
        static switchView(viewName: string) {
            if (net3000.common.currentView != undefined) {
                jQuery(net3000.common.currentView).hide();                
            }
            net3000.common.currentView = viewName;
            jQuery(viewName).show();
        }

        static pickRandomImage(urls: string[], backgroundSelector: string = "background-image") {
            var StaticImgURL = urls[Math.floor(Math.random() * urls.length)];
            if (backgroundSelector != undefined && jQuery(backgroundSelector).length > 0) {
                jQuery(backgroundSelector).css("background-image", "url(" + StaticImgURL + ")");
            }
            return StaticImgURL;
        }

        static async getUser(): Promise<loginUser> {
            if (localStorage.getItem("token") == undefined && sessionStorage.getItem("token") == undefined) {
                return null;
            }
            let res: apiResponse = await (await net3000.common.handlePromise({
                url:"user"
            })).json();
            if (res.code == 200) {
                let myContact: any = new loginUser();
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
                } else {
                    for (let key in res.data.metaDataDictionary) {
                        myContact[key] = res.data.metaDataDictionary[key];
                    }
                }
                return myContact;
            } else {
                sessionStorage.removeItem("token");
                sessionStorage.removeItem("user");
                localStorage.removeItem("token");
                localStorage.removeItem("user");
            }
            return null;
        }

        static async loadUser(parameters: { elementID: "", callBackFunction: null, redirect: null }) {
            let myUser = new loginUser();
            if (localStorage.getItem("token") != undefined || sessionStorage.getItem("token") != undefined) {
                let res: apiResponse = await (await net3000.common.handlePromise({
                    url: "user"
                })).json();
                if (res.code == 200) {
                    myUser = res.data as loginUser;
                    myUser.loggedIn = true;
                }
            }
            
            net3000.common.apps[parameters.elementID] = new Vue({
                el: "#" + parameters.elementID,
                data: {
                    client: myUser as loginUser
                },
                methods: {
                    logout: function () {
                        net3000.common.logOut(parameters.redirect);
                        this.client = new loginUser();
                    }
                }
            });
        }

        static async getAddresses(): Promise<address[]> {
            if (localStorage.getItem("token") == undefined) {
                return null;
            }
            let res: apiResponse = await (await net3000.common.handlePromise({
                url: "addresses"
            })).json();
            if (res.code == 302) {
                return res.data as address[];
            }
            return null;
        }

        static logOut(redirect: string = null): void {
            localStorage.removeItem("user");
            localStorage.removeItem("token");
            if (redirect != undefined) {
                window.location.href = redirect;
            }
        }

        static replacePlaceHolders(body: string, obj: any) {
            if (body == null || body == undefined) { return null; }
            for (let prop in obj) {
                body = body.replace(`##${prop}##`, obj[prop]);
            }
            return body;
        }

        static passwordStrength(password: string): string[] {
            let errors: string[] = [];
            if (password.length < 8) {
                errors.push("Password must be at least 8 characters.");
            }
            if (password.match(/[A-Z]/g) == null) {
                errors.push("Password must include an uppercase character.");
            }
            if (password.match(/[a-z]/g) == null) {
                errors.push("Password must include a lowercase character.");
            }
            if (password.match(/[0-9]/g) == null) {
                errors.push("Password must include a number.");
            }
            return errors;
        }

        //Global Variables
        static currentAccount: account = new net3000.account();
        static myResponse: apiResponse = new net3000.apiResponse();

        static async initializeLogin() {
            let loginTemplate: apiResponse = await (await net3000.common.handlePromise({ url: "common/setting?title=loginComponent" })).json();                          
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
                checkEmail: async function () {
                    let form: HTMLFormElement = document.querySelector("#loginForm input[name=email]");
                    if (!form.checkValidity()) {
                        this.msgBox = net3000.common.msgBox(net3000.standardMessages.invalid);
                        return;
                    }
                    let response = await (await net3000.common.handlePromise({ url: `checkEmail?email=${this.client.email}&loginGroup=${this.logingroup}` })).json();
                    if (response.code == 302) {
                        this.client.isNew = false;
                    } else {
                        this.client.isNew = true;
                    }
                },
                getAuthorizationCode: async function () {
                    this.processing = true;
                    let response = await (await net3000.common.handlePromise({ url: `sendAuthorizationCode?email=${this.client.email}&loginGroup=${this.logingroup}` })).json();
                    if (response.code == 302) {
                        this.msgBox = '';
                        this.showAuhorization = true;
                    }
                    this.processing = false;
                },
                verifyCode: async function () {
                    this.processing = true;
                    let response = await (await net3000.common.handlePromise({ url: `authorizeCode?email=${this.client.email}&code=${this.client.authorizationCode}` })).json();
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
                        this.$emit("continue");
                    } else {
                        this.showAuhorization = null;
                        this.msgBox = response.html;
                    }
                    this.processing = false;
                },
                login: async function () {
                    this.processing = true;
                    let response = await (await net3000.common.handlePromise({
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
                    } else {
                        //jQuery("#userLoginButtonContainer > *").toggle();
                        this.processing = false;
                        this.showAuhorization = null;
                        this.msgBox = response.html;
                    }
                },
                register: async function () {
                    //jQuery("#userRegisterButtonContainer > *").toggle();
                    this.processing = true;
                    let response = await (await net3000.common.handlePromise({
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
                    } else {
                        //jQuery("#userRegisterButtonContainer > *").toggle();
                        this.showAuhorization = null;
                        this.msgBox = response.html;
                    }
                    this.processing = false;
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
            mounted: async function () {
                    let user = await net3000.common.getUser();
                    if (user == null) {
                        this.client.loggedIn = false;
                    } else {
                        this.client.loggedIn = true;
                        this.client.isNew = false;
                        this.client.firstName = user.firstName;
                        this.client.lastName = user.lastName;
                        this.client.email = user.email;
                        this.client.loginGroupID = user.loginGroupID;
                        this.client.password = undefined;
                    }
            }
        });
        }

        static async initializeAccountUpdate() {
            let loginTemplate: apiResponse = await (await net3000.common.handlePromise({ url: "common/setting?title=updateAccount" })).json();
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
                        let confirmBox = document.getElementById("confirmPassword") as HTMLInputElement;
                        if (this.client.password == undefined || this.client.password == '' || this.client.password == this.client.confirmPassword) {
                            confirmBox.setCustomValidity('');
                        } else {
                            confirmBox.setCustomValidity("Password not matching");
                        }
                    },
                    updateAccount: async function () {
                        this.processing = true;
                        this.msgBox = {};
                        let AddressForm: HTMLFormElement = document.querySelector("form.clientDetails");
                        if (!AddressForm.checkValidity() || !(this.client.password == undefined || this.client.password == '' || this.client.password == this.client.confirmPassword)) {
                            jQuery("form.clientDetails").addClass("was-validated");
                            this.processing = false;
                            return;
                        } else {
                            jQuery("form.clientDetails").removeClass("was-validated");
                        }
                        let postClient = { ...this.client };
                        delete postClient.isNew;
                        delete postClient.loggedIn;
                        delete postClient.id;
                        delete postClient.metaDictionary;
                        delete postClient.mainPage;
                        delete postClient.loginGroupID;
                        let res: apiResponse = await (await (net3000.common.handlePromise({ url: "user", body: JSON.stringify(postClient), method: "put" }))).json();
                        this.msgBox = res;
                        this.processing = false;
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
        }

        static async initializeProvinceList() {
            //let loginTemplate: apiResponse = await (await net3000.common.handlePromise({ url: "common/setting?title=provinceList" })).json();
            let allStatesAndProvinces: any[] = await (await net3000.common.handlePromise({ url: "/provinces" })).json();
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
                    myProvinces: function ():any[] {
                        return this.provinces.filter((p:any) => p.countryID == this.countryid);
                    },
                    provinceTitle: function (): string {
                        if (this.countryid == "US") { return "State";}
                        if (this.countryid == "CA") { return "Province"; }
                        return "Region";
                    }
                }
            });
        }

        static async getData(parameters: any = {elementID: null, source: null, localStorage: null, sessionStorage: null}) {
            if (parameters.elementID == undefined || parameters.source == undefined) { return; }
            let dataSource: apiResponse = standardMessages.found;
            let found: boolean = false;
            if (parameters.localStorage != undefined) {
                if (localStorage.getItem(parameters.localStorage) != undefined) {
                    dataSource = JSON.parse(localStorage.getItem(parameters.localStorage));
                    found = true;
                }
            } else if (parameters.sessionStorage != undefined) {
                if (sessionStorage.getItem(parameters.sessionStorage) != undefined) {
                    dataSource = JSON.parse(sessionStorage.getItem(parameters.sessionStorage));
                    found = true;
                }
            }
            if (!found) {
                dataSource = await (await net3000.common.handlePromise({ url: parameters.source })).json();
                if (parameters.localStorage != undefined) { localStorage.setItem(parameters.localStorage, JSON.stringify(dataSource));}
                if (parameters.sessionStorage != undefined) { sessionStorage.setItem(parameters.sessionStorage, JSON.stringify(dataSource));}
            }            
            net3000.common.apps[parameters.elementID] = new Vue({
                el: "#" + parameters.elementID,
                data: dataSource
            });
        }

        static screenSize():string {
            let width = window.innerWidth;
            if (width < 576) {
                return "xs";
            } else if (width < 768) {
                return "sm";
            } else if (width < 992) {
                return "md";
            } else if (width < 1200) {
                return "lg";
            } else {
                return "xl";
            }
        }
    }
    
}

let net3000Account:number = undefined;

jQuery(function () {
    jQuery("[data-getdata]").each(function () {
        net3000.common.getData({ elementID: jQuery(this).attr("id"), source: jQuery(this).data("getdata"), localStorage: jQuery(this).data("localstorage"), sessionStorage: jQuery(this).data("sessionstorage")});
    });
});
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var Calysto;
(function (Calysto) {
    var Blazor;
    (function (Blazor) {
        var Interop;
        (function (Interop) {
            function SleepAsync(sleepMs) {
                return __awaiter(this, void 0, void 0, function* () {
                    return new Promise((success, reject) => {
                        setTimeout(() => success(), sleepMs);
                    });
                });
            }
            Interop.SleepAsync = SleepAsync;
            function ExecuteScriptAsync(js) {
                return __awaiter(this, void 0, void 0, function* () {
                    return yield (new Function(js))();
                });
            }
            Interop.ExecuteScriptAsync = ExecuteScriptAsync;
            function CreateEl(el) {
                return {
                    tagName: el.tagName,
                    id: el.id,
                    style: el.style.cssText,
                    className: el.className,
                    offsetWidth: el.offsetWidth,
                    offsetHeight: el.offsetHeight,
                    name: el.name || el.getAttribute("name"),
                    value: el.value || el.getAttribute("value"),
                    type: el.type || el.getAttribute("type")
                };
            }
            function GetElementById(id) {
                let el = document.getElementById(id);
                if (!el)
                    return null;
                else
                    return CreateEl(el);
            }
            Interop.GetElementById = GetElementById;
            function SelectElements(source, skip, take) {
                skip = skip || 0;
                take = take || Number.MAX_SAFE_INTEGER;
                let arr2 = [];
                let index = -1;
                for (let el2 of source) {
                    if (++index >= skip && index < skip + take) {
                        arr2.push(CreateEl(el2));
                    }
                }
                return arr2;
            }
            function GetElementsByTagName(tagName, skip, take) {
                return SelectElements(document.getElementsByTagName(tagName), skip, take);
            }
            Interop.GetElementsByTagName = GetElementsByTagName;
            function GetElementsByClassName(className, skip, take) {
                return SelectElements(document.getElementsByClassName(className), skip, take);
            }
            Interop.GetElementsByClassName = GetElementsByClassName;
            function GetElementsByName(className, skip, take) {
                return SelectElements(document.getElementsByName(className), skip, take);
            }
            Interop.GetElementsByName = GetElementsByName;
            //#endregion
            //#region waiting for DOM events
            function WaitForDomEventAsync(eventName, targetElementId, fnCreateEventArgs) {
                return new Promise((resolve, reject) => {
                    let el = document;
                    if (targetElementId) {
                        el = document.getElementById(targetElementId);
                    }
                    let callback = (ev) => {
                        el.removeEventListener(eventName, callback);
                        resolve(fnCreateEventArgs(ev));
                    };
                    el.addEventListener(eventName, callback);
                });
            }
            Interop.WaitForDomEventAsync = WaitForDomEventAsync;
            function WaitForMouseEventAsync(eventName, targetElementId) {
                return WaitForDomEventAsync(eventName, targetElementId, (ev) => {
                    return {
                        Detail: ev.detail,
                        ScreenX: ev.screenX,
                        ScreenY: ev.screenY,
                        ClientX: ev.clientX,
                        ClientY: ev.clientY,
                        OffsetX: ev.offsetX,
                        OffsetY: ev.offsetY,
                        Button: ev.button,
                        Buttons: ev.buttons,
                        CtrlKey: ev.ctrlKey,
                        ShiftKey: ev.shiftKey,
                        AltKey: ev.altKey,
                        MetaKey: ev.metaKey,
                        Type: ev.type
                    };
                });
            }
            Interop.WaitForMouseEventAsync = WaitForMouseEventAsync;
            function WaitForKeyboardEventAsync(eventName, targetElementId) {
                return WaitForDomEventAsync(eventName, targetElementId, (ev) => {
                    return {
                        Key: ev.key,
                        Code: ev.code,
                        Location: ev.location,
                        Repeat: ev.repeat,
                        CtrlKey: ev.ctrlKey,
                        ShiftKey: ev.shiftKey,
                        AltKey: ev.altKey,
                        MetaKey: ev.metaKey,
                        Type: ev.type
                    };
                });
            }
            Interop.WaitForKeyboardEventAsync = WaitForKeyboardEventAsync;
            //#endregion
            //#region client version control
            function RemoveBlazorOfflineCacheAsync() {
                return __awaiter(this, void 0, void 0, function* () {
                    // Delete caches
                    let cacheKeys = yield caches.keys();
                    yield Promise.all(cacheKeys
                        .filter(key => key.startsWith("offline-cache-") || key.startsWith("blazor-resources-"))
                        .map(key => caches.delete(key)));
                });
            }
            function RemoveBlazorOfflineCacheAndReloadAsync() {
                return __awaiter(this, void 0, void 0, function* () {
                    console.log("Refresh blazor offline cache");
                    yield RemoveBlazorOfflineCacheAsync();
                    location.reload();
                });
            }
            Interop.RemoveBlazorOfflineCacheAndReloadAsync = RemoveBlazorOfflineCacheAndReloadAsync;
            function GetServerVersionFromHtml() {
                // version should be writtent on server side:
                // <meta id="server-version" value="ca67ebf7-c140-4a3e-bf75-dd919399af0a"/>
                let el = document.getElementById("server-version");
                return (el === null || el === void 0 ? void 0 : el.value) || (el === null || el === void 0 ? void 0 : el.getAttribute("value"));
            }
            function InitialBlazorVersionCheckAsync() {
                return __awaiter(this, void 0, void 0, function* () {
                    // only if server-version exists in html
                    let expected1 = GetServerVersionFromHtml();
                    if (!expected1)
                        throw "Missing server-version in html";
                    let current1 = localStorage.getItem("blazor-initial");
                    if (current1 != expected1) {
                        localStorage.setItem("blazor-initial", expected1);
                        console.log("Reload blazor-initial");
                        yield RemoveBlazorOfflineCacheAndReloadAsync();
                    }
                });
            }
            InitialBlazorVersionCheckAsync();
            //#endregion
        })(Interop = Blazor.Interop || (Blazor.Interop = {}));
    })(Blazor = Calysto.Blazor || (Calysto.Blazor = {}));
})(Calysto || (Calysto = {}));
//# sourceMappingURL=Calysto.Blazor.Interop.js.map
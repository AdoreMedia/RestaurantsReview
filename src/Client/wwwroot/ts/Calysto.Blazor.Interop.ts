namespace Calysto.Blazor.Interop
{
	export async function SleepAsync(sleepMs: number)
	{
		return new Promise<void>((success, reject) =>
		{
			setTimeout(() => success(), sleepMs);
		});
	}

	export async function ExecuteScriptAsync(js: string)
	{
		return await (new Function(js))();
	}

	//#region getElementBy methods

	interface IDomElement
	{
		tagName: string;
		id: string;
		name: string;
		style: string;
		className: string;
		offsetWidth: number;
		offsetHeight: number;
		value: string;
		type: string;
	}

	function CreateEl(el: HTMLElement)
	{
		return <IDomElement>{
			tagName: el.tagName,
			id: el.id,
			style: el.style.cssText,
			className: el.className,
			offsetWidth: el.offsetWidth,
			offsetHeight: el.offsetHeight,
			name: (<any>el).name || el.getAttribute("name"),
			value: (<any>el).value || el.getAttribute("value"),
			type: (<any>el).type || el.getAttribute("type")
		};
	}

	export function GetElementById(id: string)
	{
		let el = document.getElementById(id);
		if (!el)
			return null;
		else
			return CreateEl(el);
	}

	function SelectElements(source: HTMLCollectionOf<Element>, skip?: number, take?: number)
	{
		skip = skip || 0;
		take = take || Number.MAX_SAFE_INTEGER;
		let arr2: IDomElement[] = [];
		let index = -1;
		for (let el2 of source)
		{
			if (++index >= skip && index < skip + take)
			{
				arr2.push(CreateEl(<HTMLElement>el2));
			}
		}
		return arr2;
	}

	export function GetElementsByTagName(tagName: string, skip?: number, take?: number)
	{
		return SelectElements(document.getElementsByTagName(tagName), skip, take);
	}

	export function GetElementsByClassName(className: string, skip?: number, take?: number)
	{
		return SelectElements(document.getElementsByClassName(className), skip, take);
	}

	export function GetElementsByName(className: string, skip?: number, take?: number)
	{
		return SelectElements(<any>document.getElementsByName(className), skip, take);
	}

	//#endregion

	//#region waiting for DOM events

	export function WaitForDomEventAsync(eventName: string, targetElementId: string, fnCreateEventArgs: (ev: KeyboardEvent | MouseEvent) => any)
	{
		return new Promise((resolve, reject) =>
		{
			let el = <HTMLElement><any>document;
			if (targetElementId)
			{
				el = document.getElementById(targetElementId);
			}

			let callback = (ev: Event) =>
			{
				el.removeEventListener(eventName, callback);
				resolve(fnCreateEventArgs(<any>ev));
			};

			el.addEventListener(eventName, callback);
		});
	}

	interface IMouseEventArgs
	{
		Detail: number;
		ScreenX: number;
		ScreenY: number;
		ClientX: number;
		ClientY: number;
		OffsetX: number;
		OffsetY: number;
		Button: number;
		Buttons: number;
		CtrlKey: boolean;
		ShiftKey: boolean;
		AltKey: boolean;
		MetaKey: boolean;
		Type: string;
	}

	export function WaitForMouseEventAsync(eventName: string, targetElementId?: string)
	{
		return WaitForDomEventAsync(eventName, targetElementId, (ev: MouseEvent) =>
		{
			return <IMouseEventArgs>{
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

	interface IKeyboardEventArgs
	{
		Key: string;
		Code: string;
		Location: number;
		Repeat: boolean;
		CtrlKey: boolean;
		ShiftKey: boolean;
		AltKey: boolean;
		MetaKey: boolean;
		Type: string;
	}

	export function WaitForKeyboardEventAsync(eventName: string, targetElementId?: string)
	{
		return WaitForDomEventAsync(eventName, targetElementId, (ev: KeyboardEvent) =>
		{
			return <IKeyboardEventArgs>{
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

	//#endregion

	//#region client version control

	async function RemoveBlazorOfflineCacheAsync()
	{
		// Delete caches
		let cacheKeys = await caches.keys();
		await Promise.all(cacheKeys
			.filter(key => key.startsWith("offline-cache-") || key.startsWith("blazor-resources-"))
			.map(key => caches.delete(key)));
	}

	export async function RemoveBlazorOfflineCacheAndReloadAsync()
	{
		console.log("Refresh blazor offline cache");
		await RemoveBlazorOfflineCacheAsync();
		location.reload();
	}

	function GetServerVersionFromHtml()
	{
		// version should be writtent on server side:
		// <meta id="server-version" value="ca67ebf7-c140-4a3e-bf75-dd919399af0a"/>
		let el = <any> document.getElementById("server-version");
		return el?.value || el?.getAttribute("value");
	}

	async function InitialBlazorVersionCheckAsync()
	{
		// only if server-version exists in html
		let expected1 = GetServerVersionFromHtml();
		if (!expected1)
			throw "Missing server-version in html";

		let current1 = localStorage.getItem("blazor-initial");

		if (current1 != expected1)
		{
			localStorage.setItem("blazor-initial", expected1);
			console.log("Reload blazor-initial");

			await RemoveBlazorOfflineCacheAndReloadAsync();
		}
	}

	InitialBlazorVersionCheckAsync();

	//#endregion

}
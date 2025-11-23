declare const document: any;

export const createHiddenElement = (
  id: string,
  value: string,
  parentSelector: string
): void => {
  var input = document.createElement("input");
  input.setAttribute("type", "hidden");
  input.setAttribute("id", id);
  input.setAttribute("name", id);
  input.setAttribute("value", value);

  //append to form element that you want.
  let selector = document.querySelector(parentSelector);
  if (selector) {
    selector.appendChild(input);
  }
};

export const getHiddenElementValue = (id: string): string | null => {
  var elem: any = document.getElementById(id);
  if (elem) {
    return elem.value;
  }
  return null;
};

export const removeElement = (id: string): void => {
  var elem: any = document.getElementById(id);
  if (elem) {
    elem.parentNode.removeChild(elem);
  }
};

export const setElemDisplay = (elem: any, display: string) => {
  if (elem) {
    elem.style.display = display;
  }
};

export const addClass = (selector: any, className: string) => {
  let elem = document.querySelector(selector);
  if (elem) {
    let css = elem.className;
    css = css.replace(className, "");
    css = `${css.trim()} ${className}`.trim();
    elem.className = css ? css.trim() : css;
  }
};

export const addElemClass = (elem: any, className: string) => {
  if (elem) {
    let css = elem.className;
    css = css.replace(className, "");
    css = `${css.trim()} ${className}`.trim();
    elem.className = css ? css.trim() : css;
  }
};

export const removeClass = (selector: any, className: string) => {
  let elem = document.querySelector(selector);
  if (elem) {
    let css = elem.className;
    css = css.replace(className, "");
    elem.className = css ? css.trim() : css;
  }
};

export const removeElemClass = (elem: any, className: string) => {
  if (elem) {
    let css = elem.className;
    css = css.replace(className, "");
    elem.className = css ? css.trim() : css;
  }
};

export const setDropDownValue = (selector: any, selectedIndex: number) => {
  let elem = document.querySelector(selector);
  if (elem) {
    elem.selectedIndex = selectedIndex;
  }
};

export const numberFormatWithCommas = (x: any) => {
  return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

export const scrollToTop = () => {
  window.scrollTo(0, 0);
};

export const scrollToBottom = () => {
  window.scrollTo({
    top: document.body.scrollHeight,
    behavior: "auto",
  });
};

export const removeUKPostCodeMask = (selector: string) => {

  const elem: Element = document.querySelector(selector);
  if (elem) {
    elem.removeEventListener("keydown", () => {}, true);
    elem.removeEventListener("keyup", () => {}, true);
    elem.removeEventListener("blur", () => {}, true);
  }
};


export const removeItemOnce = (arr: any, value: any) => {
  var index = arr.indexOf(value);
  if (index > -1) {
    arr.splice(index, 1);
  }
  return arr;
}

export const removeItemAll = (arr: any, value: any) => {
  var i = 0;
  while (i < arr.length) {
    if (arr[i] === value) {
      arr.splice(i, 1);
    } else {
      ++i;
    }
  }
  return arr;
}
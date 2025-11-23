// Base

// Model Imports

// Other Imports

export default class StorageService {
  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {}

  getStringStorage(key: string): string | null {
    return localStorage.getItem(key);
  }

  getBooleanStorage(key: string): boolean | null {
    const value = localStorage.getItem(key);
    if (value) {
      return JSON.parse(value) as boolean;
    }
    return null;
  }

  getNumberStorage(key: string): number | null {
    const value = localStorage.getItem(key);
    if (value) {
      return JSON.parse(value) as number;
    }
    return null;
  }

  getObjectStorage(key: string): object | null {
    const value = localStorage.getItem(key);
    if (value) {
      return JSON.parse(value);
    }
    return null;
  }

  setStringStorage(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  setBooleanStorage(key: string, value: boolean): void {
    localStorage.setItem(key, value.toString());
  }

  setNumberStorage(key: string, value: number): void {
    localStorage.setItem(key, value.toString());
  }

  setObjectStorage(key: string, value: object): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  removeStorage(key: string): void {
    localStorage.removeItem(key);
  }
}

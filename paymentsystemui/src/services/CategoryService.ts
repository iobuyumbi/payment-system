import BaseService from "./BaseService";

export default class CategoryService extends BaseService {
  constructor() {
    super();
  }

  getItemCategoryData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(
      this.getURL("api/ItemCategory/Search"),
      data
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };

  postItemCategoryData = async (data: any): Promise<any | null> => {

    const response = await this.post(
      this.getURL("api/ItemCategory"),
      data
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return null;
  };

  putItemCategoryData = async (data: any,id:any): Promise<any | null> => {

    const response = await this.put(
      this.getURL(`api/ItemCategory/${id}`),
      data,id
    );
    if (response && this.isSuccessResponse(response)) {

      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return null;
  };
}

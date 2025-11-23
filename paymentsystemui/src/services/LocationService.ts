import BaseService from "./BaseService";

export default class LocationService extends BaseService {
  constructor() {
    super();
  }
getLocations = async (data: any): Promise<any[]> => {
  const response = await this.post(this.getURL("api/LocationProfiles/search"), data);

  if (response && this.isSuccessResponse(response)) {
    const responseData = response.data;
    if (responseData && responseData.success) {
      return responseData.data.result as any[];
    }
  }

  return [];
};

  saveLocation = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/LocationProfiles"),
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

  updateLocation = async (id: any, data: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/LocationProfiles/${id}`), data);
    
    if (response && this.isSuccessResponse(response)) {
      if (response.data && response.data.result) {
        return response.data.result as any[];
      }
    }
    
    return null;
  };


  getById = async (id: any): Promise<any> => {
  const response = await this.get(this.getURL(`api/LocationProfiles/${id}`), );
debugger
  if (response && this.isSuccessResponse(response)) {
    const responseData = response.data;
    if (responseData && responseData.succeeded) {
      return responseData.result as any;
    }
  }

  return [];
};
}

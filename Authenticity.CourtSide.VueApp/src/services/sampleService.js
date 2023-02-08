import apiService from "./apiService.js";
import {
  buildMocks,
  generateText,
  generateInteger,
  generateFloating,
} from "./mock";

//
// API Url
//

const URL = {
  GetSomeData: "ServiceManager/GetSomeData",
};

//
// API definition
//

export default {
  GetSomeDataAsync() {
    return apiService.get(URL.GetSomeData);
  },
};

//
// API mocks
//

buildMocks((mock) => {
  mock.onAny(URL.GetSomeData).reply((config) => {
    const mockData = {
      Id: generateInteger(),
      Name: generateText(),
      Value: generateFloating(),
    };

    return [200, mockData];
  });
});

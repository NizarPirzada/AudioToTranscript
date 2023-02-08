import MockAdapter from "axios-mock-adapter";
import apiService from "../apiService.js";
import axiosSettings from "@/globalSettings";

let mockAdapterInstance = null;

const buildMocks = (fn) => {
  const enableMocks = axiosSettings.mock && axiosSettings.mock.enable === true;
  if (enableMocks && typeof fn === "function") {
    if (!mockAdapterInstance) {
      mockAdapterInstance = new MockAdapter(
        apiService,
        axiosSettings.mock.adapterOptions
      );
    }
    fn(mockAdapterInstance);
  }
};

const generateInteger = (min = 0, max = 1000) =>
  Math.floor(Math.random() * (max - min + 1) + min);

const generateFloating = (min = 0, max = 1000, toFixed = 2) =>
  (Math.random() * (max - min) + min).toFixed(toFixed);

const generateText = (
  length = 8,
  characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
) => {
  let result = "";
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * characters.length));
  }
  return result;
};

export { buildMocks, generateText, generateInteger, generateFloating };

import axios from "axios";
/**
 * Khởi tạo instance axiois
 * author : LQHUY(05/12/2023)
 */
const httpRequest = axios.create({
  baseURL: "https://localhost:7291/api/v1/",
});
/**
 * Hàm gọi API lấy dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns object
 * * author : LQHUY(05/12/2023)
 */
export const get = async (path, options = {}) => {
  const res = await httpRequest.get(path, options);
  return res;
};

export const getAuthorization = async (path, options = {}) => {
  const token = JSON.parse(localStorage.getItem("Token"));
  const headers = {
    Authorization: `Bearer ${token.AccessToken}`,
    "Content-Type": "application/json",
  };
  const res = await httpRequest.get(path, { headers, ...options });
  return res;
};

/**
 * Hàm gọi API xóa dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns int`
 * * author : LQHUY(05/12/2023)
 */
export const remove = async (path, options = {}) => {
  const res = await httpRequest.delete(path, options);
  return res;
};

/**
 * Hàm gọi API thêm dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns int
 * * author : LQHUY(05/12/2023)
 */
export const post = async (path, options = {}) => {
  const res = await httpRequest.post(path, options);
  return res;
};

/**
 * Hàm gọi API xóa sửa dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns int
 * * author : LQHUY(05/12/2023)
 */
export const put = async (path, options = {}) => {
  const res = await httpRequest.put(path, options);
  return res;
};

/**
 * Hàm gọi API xuất khẩu dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns object
 * * author : LQHUY(16/01/2023)
 */
export const getExcel = async (path, options) => {
  const res = await httpRequest.post(path, options, {
    responseType: "blob",
    headers: {
      "Content-Type": "application/json",
    },
  });
  return res;
};

/**
 * Hàm gọi API nhập khẩu dữ liệu
 * @param {string} path
 * @param {object} options
 * @returns object
 * * author : LQHUY(16/01/2023)
 */
export const postExcel = async (path, options) => {
  const res = await httpRequest.post(path, options, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  return res;
};

export const importExcel = async (path, option) => {
  const res = await httpRequest.post(path, option, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  return res;
};

export default httpRequest;

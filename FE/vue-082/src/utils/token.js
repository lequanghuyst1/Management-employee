import router from "@/router/route";
import httpRequest from "./request";
import Resource from "@/js/resource/resource";
import app from "@/main.js";

export const getRefreshToken = async (emitter) => {
  const token = JSON.parse(localStorage.getItem("Token"));
  const now = new Date();
  const appConfig = app.config.globalProperties;

  const expriedAccessTokenString = token.ExpiredAccessToken;
  const expriedAccessToken = new Date(expriedAccessTokenString);
  if (expriedAccessToken < now) {
    try {
      const res = await httpRequest.post("Authentications/RenewToken", token);
      switch (res.status) {
        case 200:
          localStorage.setItem("Token", JSON.stringify(res.data));
          break;
      }
    } catch (error) {
      emitter.handleApiError(error);
    }
  }

  let expriedRefeshTokenString = token.ExpiredRefreshToken;
  let expriedRefeshToken = new Date(expriedRefeshTokenString);
  if (token && expriedRefeshToken <= now) {
    router.push("/login");
    emitter.emit(
      "onShowToastMessage",
      Resource[appConfig.$languageCode].ToastMessage.Type.Warning,
      Resource[appConfig.$languageCode].Text.AccessTokenExpired,
      Resource[appConfig.$languageCode].ToastMessage.Status.Warning
    );
    localStorage.setItem("Token", JSON.stringify(null));
  }
};

export const checkExpiredTokenTime = async (emitter) => {
  let token = JSON.parse(localStorage.getItem("Token"));
  let now = new Date();
  let expriedAccessTokenString = token.ExpiredAccessToken;
  let expriedAccessToken = new Date(expriedAccessTokenString);
  if (token && expriedAccessToken <= now) {
    await getRefreshToken(emitter);
  }
};

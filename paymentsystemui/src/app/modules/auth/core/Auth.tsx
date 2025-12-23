
/* eslint-disable react-refresh/only-export-components */
import {
  FC,
  useState,
  useEffect,
  createContext,
  useContext,
  Dispatch,
  SetStateAction,
} from "react";
import { LayoutSplashScreen } from "../../../../_metronic/layout/core";
import { AuthModel, UserModel } from "./_models";
import * as authHelper from "./AuthHelpers";
import { login, getUserByToken } from "./_requests";
import { WithChildren } from "../../../../_metronic/helpers";
import { setSelectedCountryCode } from "../../../../_metronic/helpers/AppUtil";
import PermissionService from "../../../../services/PermissionService";
import config from "../../../../environments/config";

type AuthContextProps = {
  auth: AuthModel | undefined;
  saveAuth: (auth: AuthModel | undefined) => void;
  currentUser: UserModel | undefined;
  setCurrentUser: Dispatch<SetStateAction<UserModel | undefined>>;
  logout: () => void;
};

const initAuthContextPropsState = {
  auth: authHelper.getAuth(),
  saveAuth: () => {},
  currentUser: undefined,
  setCurrentUser: () => {},
  logout: () => {},
};

const AuthContext = createContext<AuthContextProps>(initAuthContextPropsState);

const useAuth = () => {
  return useContext(AuthContext);
};

const AuthProvider: FC<WithChildren> = ({ children }) => {
  const [auth, setAuth] = useState<AuthModel | undefined>(authHelper.getAuth());
  const [currentUser, setCurrentUser] = useState<UserModel | undefined>();
  const saveAuth = (auth: AuthModel | undefined) => {
    setAuth(auth);
    if (auth) {
      authHelper.setAuth(auth);
    } else {
      authHelper.removeAuth();
    }
  };

  const logout = () => {
    saveAuth(undefined);
    setCurrentUser(undefined);
  };

  return (
    <AuthContext.Provider
      value={{ auth, saveAuth, currentUser, setCurrentUser, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

const AuthInit: FC<WithChildren> = ({ children }) => {
  const { auth, currentUser, logout, setCurrentUser, saveAuth } = useAuth();
  const [showSplashScreen, setShowSplashScreen] = useState(true);

  // We should request user by authToken (IN OUR EXAMPLE IT'S API_TOKEN) before rendering the application
  useEffect(() => {
    const requestUser = async (apiToken: string) => {
      try {
        if (auth && (!currentUser || !auth.permissions)) {
          const { data } = await getUserByToken(apiToken);
          if (data) {
            const userData = data as any;
            const permissionService = new PermissionService();
            let perms = userData.permissions;
            if (!perms || (Array.isArray(perms) && perms.length === 0)) {
              try {
                const fetched = await permissionService.GetPermissions(
                  userData.username
                );
                if (Array.isArray(fetched)) {
                  perms = fetched;
                }
              } catch {}
            }
            const updatedAuth: AuthModel = {
              ...auth,
              permissions: perms,
              countries: [{ code: "KE", name: "Kenya", id: "default" }],
            };
            saveAuth(updatedAuth);
            setCurrentUser(data);
          }
        }
      } catch (error) {
        try {
          const status = (error as any)?.response?.status;
          if (status === 401) {
            logout();
            return;
          }
          if (config.runningEnv.toLowerCase() === "local") {
            const fallbackPerms = [
              "farmers.view",
              "farmers.kyc.view",
              "loans.batch.view",
              "loans.applications.view",
              "payments.batch.history",
              "reports.general.loan-accounts",
              "farmers.import",
              "logs.email",
              "logs.contacts-api",
              "logs.payments-api",
              "logs.payments-callbacks",
              "settings.loans.categories.view",
              "settings.loans.items.view",
              "settings.loans.masterfee.view",
              "settings.loans.terms.view",
              "settings.projects.view",
              "settings.system.users.groups.view",
              "settings.system.users.view",
              "settings.cooperatives.view",
              "settings.administrative.view",
              "settings.communication.email.setup",
              "settings.communication.sms.setup",
              "settings.communication.email.template.view",
              "dashboard.viewactivitylog",
            ];
            if (auth) {
              const updatedAuth: AuthModel = {
                ...auth,
                permissions: fallbackPerms,
                countries: [{ code: "KE", name: "Kenya", id: "default" }],
              };
              saveAuth(updatedAuth);
            }
          } else {
            if (currentUser) {
              logout();
            }
          }
        } catch {}
      } finally {
        setShowSplashScreen(false);
      }
    };

    if (auth?.api_token) {
      setSelectedCountryCode("KE");
      requestUser(auth.api_token);
    } else {
      logout();
      setShowSplashScreen(false);
    }
  }, []);

  return showSplashScreen ? <LayoutSplashScreen /> : <>{children}</>;
};

export { AuthProvider, AuthInit, useAuth };

import { createRouter, createWebHistory } from "vue-router";
const CustomerPage = () => import("@/view/customer/Index.vue");
const EmployeePage = () => import("@/view/employee/Index.vue");
const LoginPage = () => import("@/view/login-page/LoginPage.vue");
const DefaultLayout = () =>
  import("@/components/layout/layout-default/Index.vue");
const FullWithLayout = () =>
  import("@/components/layout/layout-fullwidth/Index.vue");
import { menu } from "@/js/resource/menu";
import PageNotFound from "@/view/page-not-found/PageNotFound.vue";

// const routes = [
//   {
//     path: "/customer",
//     name: "CustomerRouter",
//     components: { default: CustomerPage, CustomerRouterView: EmployeePage },
//     children: [
//       {
//         path: "",
//         name: "CustomerRouterList",
//         components: {
//           default: CustomerPage,
//           CustomerRouterView: CustomerList,
//         },
//       },
//       {
//         path: "create",
//         name: "CustomerRouterDetail",
//         components: {
//           default: CustomerPage,
//           CustomerRouterView: CustomerDetail,
//         },
//       },
//       {
//         path: ":id",
//         name: "CustomerRouterAdd",
//         components: {
//           default: CustomerPage,
//           CustomerRouterView: CustomerDetail,
//         },
//       },
//     ],
//   },
//   { path: "/employee", name: "EmployeeRouter", component: EmployeePage },
// ];
const routes = [
  {
    path: "/",
    components: { default: DefaultLayout, LayoutViewRouter: DefaultLayout },
    meta: { requiresAuth: true },
    children: [
      {
        path: "home",
        components: {
          default: DefaultLayout,
          ContentRouterView: EmployeePage,
        },
      },
      {
        path: "customer",
        components: {
          default: DefaultLayout,
          ContentRouterView: CustomerPage,
        },
      },
      {
        path: "employee",
        components: {
          default: DefaultLayout,
          ContentRouterView: EmployeePage,
        },
      },
    ],
  },
  {
    path: "/login",
    components: {
      default: FullWithLayout,
      LayoutViewRouter: LoginPage,
    },
  },
  {
    path: "/page-not-found",
    components: {
      default: FullWithLayout,
      LayoutViewRouter: PageNotFound,
    },
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const checkRouter = menu.map((item) => item.path).includes(to.path);
  const token = JSON.parse(localStorage.getItem("Token"));
  if (to.path !== "/login" && to.path !== "/" && !checkRouter) {
    next("/page-not-found");
  }
  if (to.meta.requiresAuth && !token) {
    next("/login"); // Chuyển hướng đến trang login nếu cố gắng truy cập vào trang user
  } else if (to.path === "/login" && token) {
    next("/employee");
  } else {
    next();
  }
});
export default router;

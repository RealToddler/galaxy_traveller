"use strict";
/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
self["webpackHotUpdate_N_E"]("pages/team",{

/***/ "./components/MemberCard.tsx":
/*!***********************************!*\
  !*** ./components/MemberCard.tsx ***!
  \***********************************/
/***/ (function(module, __webpack_exports__, __webpack_require__) {

eval(__webpack_require__.ts("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"./node_modules/react/jsx-dev-runtime.js\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var next_image__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! next/image */ \"./node_modules/next/image.js\");\n/* harmony import */ var next_image__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(next_image__WEBPACK_IMPORTED_MODULE_1__);\n\n\nconst MemberCard = ()=>{\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.Fragment, {\n        children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n            className: \"w-72 bg-rosequartz h-96 rounded-lg p-4 space-y-2 text-justify\",\n            children: [\n                /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)((next_image__WEBPACK_IMPORTED_MODULE_1___default()), {\n                    src: \"/asset2.png\",\n                    alt: \"self\",\n                    width: 500,\n                    height: 500,\n                    className: \"h-3/5 rounded\"\n                }, void 0, false, {\n                    fileName: \"D:\\\\github\\\\cygnus_website\\\\components\\\\MemberCard.tsx\",\n                    lineNumber: 7,\n                    columnNumber: 9\n                }, undefined),\n                /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"h1\", {\n                    className: \"text-lg\",\n                    children: \"FIRSTNAME Name\"\n                }, void 0, false, {\n                    fileName: \"D:\\\\github\\\\cygnus_website\\\\components\\\\MemberCard.tsx\",\n                    lineNumber: 9,\n                    columnNumber: 9\n                }, undefined),\n                /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                    className: \"text-\",\n                    children: \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Id ex, consectetur optio atque impedit adipisci.\"\n                }, void 0, false, {\n                    fileName: \"D:\\\\github\\\\cygnus_website\\\\components\\\\MemberCard.tsx\",\n                    lineNumber: 10,\n                    columnNumber: 9\n                }, undefined)\n            ]\n        }, void 0, true, {\n            fileName: \"D:\\\\github\\\\cygnus_website\\\\components\\\\MemberCard.tsx\",\n            lineNumber: 6,\n            columnNumber: 7\n        }, undefined)\n    }, void 0, false);\n};\n_c = MemberCard;\n/* harmony default export */ __webpack_exports__[\"default\"] = (MemberCard);\nvar _c;\n$RefreshReg$(_c, \"MemberCard\");\n\n\n;\n    // Wrapped in an IIFE to avoid polluting the global scope\n    ;\n    (function () {\n        var _a, _b;\n        // Legacy CSS implementations will `eval` browser code in a Node.js context\n        // to extract CSS. For backwards compatibility, we need to check we're in a\n        // browser context before continuing.\n        if (typeof self !== 'undefined' &&\n            // AMP / No-JS mode does not inject these helpers:\n            '$RefreshHelpers$' in self) {\n            // @ts-ignore __webpack_module__ is global\n            var currentExports = module.exports;\n            // @ts-ignore __webpack_module__ is global\n            var prevSignature = (_b = (_a = module.hot.data) === null || _a === void 0 ? void 0 : _a.prevSignature) !== null && _b !== void 0 ? _b : null;\n            // This cannot happen in MainTemplate because the exports mismatch between\n            // templating and execution.\n            self.$RefreshHelpers$.registerExportsForReactRefresh(currentExports, module.id);\n            // A module can be accepted automatically based on its exports, e.g. when\n            // it is a Refresh Boundary.\n            if (self.$RefreshHelpers$.isReactRefreshBoundary(currentExports)) {\n                // Save the previous exports signature on update so we can compare the boundary\n                // signatures. We avoid saving exports themselves since it causes memory leaks (https://github.com/vercel/next.js/pull/53797)\n                module.hot.dispose(function (data) {\n                    data.prevSignature =\n                        self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports);\n                });\n                // Unconditionally accept an update to this module, we'll check if it's\n                // still a Refresh Boundary later.\n                // @ts-ignore importMeta is replaced in the loader\n                module.hot.accept();\n                // This field is set when the previous version of this module was a\n                // Refresh Boundary, letting us know we need to check for invalidation or\n                // enqueue an update.\n                if (prevSignature !== null) {\n                    // A boundary can become ineligible if its exports are incompatible\n                    // with the previous exports.\n                    //\n                    // For example, if you add/remove/change exports, we'll want to\n                    // re-execute the importing modules, and force those components to\n                    // re-render. Similarly, if you convert a class component to a\n                    // function, we want to invalidate the boundary.\n                    if (self.$RefreshHelpers$.shouldInvalidateReactRefreshBoundary(prevSignature, self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports))) {\n                        module.hot.invalidate();\n                    }\n                    else {\n                        self.$RefreshHelpers$.scheduleUpdate();\n                    }\n                }\n            }\n            else {\n                // Since we just executed the code for the module, it's possible that the\n                // new exports made it ineligible for being a boundary.\n                // We only care about the case when we were _previously_ a boundary,\n                // because we already accepted this update (accidental side effect).\n                var isNoLongerABoundary = prevSignature !== null;\n                if (isNoLongerABoundary) {\n                    module.hot.invalidate();\n                }\n            }\n        }\n    })();\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9jb21wb25lbnRzL01lbWJlckNhcmQudHN4IiwibWFwcGluZ3MiOiI7Ozs7OztBQUErQjtBQUUvQixNQUFNQyxhQUFhO0lBQ2pCLHFCQUNFO2tCQUNFLDRFQUFDQztZQUFJQyxXQUFVOzs4QkFDYiw4REFBQ0gsbURBQUtBO29CQUFDSSxLQUFJO29CQUFjQyxLQUFJO29CQUFPQyxPQUFPO29CQUFLQyxRQUFRO29CQUN4REosV0FBVTs7Ozs7OzhCQUNWLDhEQUFDSztvQkFBR0wsV0FBVTs4QkFBVTs7Ozs7OzhCQUN4Qiw4REFBQ0Q7b0JBQUlDLFdBQVU7OEJBQVE7Ozs7Ozs7Ozs7Ozs7QUFNL0I7S0FiTUY7QUFlTiwrREFBZUEsVUFBVUEsRUFBQyIsInNvdXJjZXMiOlsid2VicGFjazovL19OX0UvLi9jb21wb25lbnRzL01lbWJlckNhcmQudHN4P2QxZDYiXSwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IEltYWdlIGZyb20gXCJuZXh0L2ltYWdlXCI7XG5cbmNvbnN0IE1lbWJlckNhcmQgPSAoKSA9PiB7XG4gIHJldHVybiAoXG4gICAgPD5cbiAgICAgIDxkaXYgY2xhc3NOYW1lPVwidy03MiBiZy1yb3NlcXVhcnR6IGgtOTYgcm91bmRlZC1sZyBwLTQgc3BhY2UteS0yIHRleHQtanVzdGlmeVwiPlxuICAgICAgICA8SW1hZ2Ugc3JjPVwiL2Fzc2V0Mi5wbmdcIiBhbHQ9XCJzZWxmXCIgd2lkdGg9ezUwMH0gaGVpZ2h0PXs1MDB9IFxuICAgICAgICBjbGFzc05hbWU9XCJoLTMvNSByb3VuZGVkXCIvPlxuICAgICAgICA8aDEgY2xhc3NOYW1lPVwidGV4dC1sZ1wiPkZJUlNUTkFNRSBOYW1lPC9oMT5cbiAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJ0ZXh0LVwiPlxuICAgICAgICAgICAgTG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQgY29uc2VjdGV0dXIgYWRpcGlzaWNpbmcgZWxpdC4gSWQgZXgsIGNvbnNlY3RldHVyIG9wdGlvIGF0cXVlIGltcGVkaXQgYWRpcGlzY2kuXG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9kaXY+IFxuICAgIDwvPlxuICApO1xufTtcblxuZXhwb3J0IGRlZmF1bHQgTWVtYmVyQ2FyZDtcbiJdLCJuYW1lcyI6WyJJbWFnZSIsIk1lbWJlckNhcmQiLCJkaXYiLCJjbGFzc05hbWUiLCJzcmMiLCJhbHQiLCJ3aWR0aCIsImhlaWdodCIsImgxIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./components/MemberCard.tsx\n"));

/***/ })

});
import React from 'react';
import ReactDOM from 'react-dom';
import 'semantic-ui-css/semantic.min.css';
import 'react-calendar/dist/Calendar.css';
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-datepicker/dist/react-datepicker.css'
import './app/layout/styles.css';
import App from './app/layout/App';
import reportWebVitals from './reportWebVitals';
import { store, StoreContext } from './app/stores/store';
import { Router, Route } from 'react-router-dom';
import {createBrowserHistory} from 'history';
import ScrollToTop from './app/layout/ScrollToTop';
import { QueryParamProvider } from "use-query-params";
import { GlobalStyle, theme } from './styles';
import { ThemeProvider } from "styled-components";

export let history = createBrowserHistory();

ReactDOM.render(
  <ThemeProvider theme={theme}>
  <StoreContext.Provider value={store}>
  <Router history={history}>
  <QueryParamProvider ReactRouterRoute={Route}>
    <ScrollToTop />
    <App />
    <GlobalStyle />
    </QueryParamProvider>
    </Router>
  </StoreContext.Provider>
  </ThemeProvider>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

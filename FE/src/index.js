import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';


ReactDOM.render(<App />, document.getElementById('root'));


//const DO_NOT_LOGIN = false;

//runWithAdal(authContext, () => {
  ReactDOM.render(
      <App />,
      document.getElementById('root')
  );
  serviceWorker.unregister();
  //registerServiceWorker();
//},DO_NOT_LOGIN);



//serviceWorker.unregister();

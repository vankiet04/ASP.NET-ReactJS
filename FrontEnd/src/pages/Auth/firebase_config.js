// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyB_HCaCKuny9YEKamSaAROdxyj2YiAR_pQ",
  authDomain: "ecommercaspreactjs.firebaseapp.com",
  projectId: "ecommercaspreactjs",
  storageBucket: "ecommercaspreactjs.firebasestorage.app",
  messagingSenderId: "965527708253",
  appId: "1:965527708253:web:d8cf294f1ee0eb36c023b4",
  measurementId: "G-YD2904WQ30"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
export default app;
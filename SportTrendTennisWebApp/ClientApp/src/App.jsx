import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import TennisRegistrationMUI from "./components/main-component";
import './App.css';
import LoginContainer from "./pages/login/login-container";
import { SignupContainer } from "./pages/signup/signup-container";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginContainer />} />
        <Route path="/signup" element={<SignupContainer />} />
        <Route path="/main" element={<TennisRegistrationMUI />} />
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;

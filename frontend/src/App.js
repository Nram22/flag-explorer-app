// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CountryProvider } from './context/CountryContext';
import Home from './pages/Home';
import Detail from './pages/Detail';

function App() {
  return (
    <CountryProvider>
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/country/:name" element={<Detail />} />
        </Routes>
      </Router>
    </CountryProvider>
  );
}

export default App;

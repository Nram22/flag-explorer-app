// frontend/src/context/CountryContext.js
import React, { createContext, useState, useEffect } from 'react';

export const CountryContext = createContext();

export const CountryProvider = ({ children }) => {
  const [countries, setCountries] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchCountries = () => {
    setLoading(true);
    fetch('http://localhost:5083/api/countries')
      .then(response => response.json())
      .then(data => {
        setCountries(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching countries:', error);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchCountries();
  }, []);

  // Expose refresh function.
  const refreshCountries = () => {
    fetchCountries();
  };

  return (
    <CountryContext.Provider value={{ countries, loading, refreshCountries }}>
      {children}
    </CountryContext.Provider>
  );
};

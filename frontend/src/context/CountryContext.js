// frontend/src/context/CountryContext.js
import React, { createContext, useState, useEffect } from 'react';

export const CountryContext = createContext();

export const CountryProvider = ({ children }) => {
  const [countries, setCountries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchCountries = () => {
    setLoading(true);
    setError(null);
    fetch('http://localhost:5083/api/countries')
      .then(response => {
        if (!response.ok) {
          throw new Error("API not available");
        }
        return response.json();
      })
      .then(data => {
        setCountries(data);
        setLoading(false);
      })
      .catch(err => {
        console.error('Error fetching countries:', err);
        setError(err);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchCountries();
  }, []);

  const refreshCountries = () => {
    fetchCountries();
  };

  return (
    <CountryContext.Provider value={{ countries, loading, error, refreshCountries }}>
      {children}
    </CountryContext.Provider>
  );
};

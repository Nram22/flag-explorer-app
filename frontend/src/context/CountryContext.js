// frontend/src/context/CountryContext.js
import React, { createContext, useState, useEffect } from 'react';

export const CountryContext = createContext();

// CountryProvider fetches the list of countries from the backend API and provides it via context.
export const CountryProvider = ({ children }) => {
  const [countries, setCountries] = useState([]);
  const [loading, setLoading] = useState(true);

  // Fetch countries on component mount.
  useEffect(() => {
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
  }, []);

  return (
    <CountryContext.Provider value={{ countries, loading }}>
      {children}
    </CountryContext.Provider>
  );
};

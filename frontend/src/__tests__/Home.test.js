// frontend/src/__tests__/Home.test.js
import React from 'react';
import { render, screen } from '@testing-library/react';
import Home from '../pages/Home';
import { CountryContext } from '../context/CountryContext';
import { BrowserRouter as Router } from 'react-router-dom';

// Tests for the Home page.
describe('Home Page', () => {
  test('renders loading message when data is loading', () => {
    render(
      <CountryContext.Provider value={{ countries: [], loading: true }}>
        <Router>
          <Home />
        </Router>
      </CountryContext.Provider>
    );
    expect(screen.getByText(/Loading/i)).toBeInTheDocument();
  });

  test('renders country cards when data is loaded', () => {
    const testCountries = [
      { name: 'France', flag: 'https://flagcdn.com/fr.png' },
      { name: 'Germany', flag: 'https://flagcdn.com/de.png' }
    ];
    render(
      <CountryContext.Provider value={{ countries: testCountries, loading: false }}>
        <Router>
          <Home />
        </Router>
      </CountryContext.Provider>
    );
    expect(screen.getByText('France')).toBeInTheDocument();
    expect(screen.getByText('Germany')).toBeInTheDocument();
  });
});

// frontend/src/__tests__/Home.test.js
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Home from '../pages/Home';
import { CountryContext } from '../context/CountryContext';
import { BrowserRouter as Router } from 'react-router-dom';

describe('Home Page', () => {
  test('renders loading message when data is loading', () => {
    render(
      <CountryContext.Provider value={{ countries: [], loading: true, refreshCountries: jest.fn() }}>
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
      <CountryContext.Provider value={{ countries: testCountries, loading: false, refreshCountries: jest.fn() }}>
        <Router>
          <Home />
        </Router>
      </CountryContext.Provider>
    );
    expect(screen.getByText('France')).toBeInTheDocument();
    expect(screen.getByText('Germany')).toBeInTheDocument();
  });

  test('calls refreshCountries when refresh button is clicked', () => {
    const mockRefresh = jest.fn();
    const testCountries = [
      { name: 'France', flag: 'https://flagcdn.com/fr.png' },
      { name: 'Germany', flag: 'https://flagcdn.com/de.png' }
    ];
    render(
      <CountryContext.Provider value={{ countries: testCountries, loading: false, refreshCountries: mockRefresh }}>
        <Router>
          <Home />
        </Router>
      </CountryContext.Provider>
    );
    const refreshButton = screen.getByRole('button', { name: /refresh/i });
    fireEvent.click(refreshButton);
    expect(mockRefresh).toHaveBeenCalled();
  });

  test('displays error state with placeholder image and error message when API is unavailable', () => {
    const mockRefresh = jest.fn();
    render(
      <CountryContext.Provider value={{ countries: [], loading: false, error: new Error("API not available"), refreshCountries: mockRefresh }}>
        <Router>
          <Home />
        </Router>
      </CountryContext.Provider>
    );
    
    // Verify that the error message is displayed.
    expect(screen.getByText(/Unable to load country data/i)).toBeInTheDocument();
    
    // Verify that the refresh button is displayed and works.
    const refreshButton = screen.getByRole('button', { name: /refresh/i });
    fireEvent.click(refreshButton);
    expect(mockRefresh).toHaveBeenCalled();
    
    // Verify that the placeholder image is present (assuming the alt text "Placeholder" is used).
    expect(screen.getByAltText(/Placeholder/i)).toBeInTheDocument();
  });
});

// src/__tests__/Detail.test.js
import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import Detail from '../pages/Detail';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

// Update the mock to use a case-insensitive URL match:
global.fetch = jest.fn((url) => {
  if (typeof url === 'string' && url.toLowerCase().includes('/api/countries/france')) {
    return Promise.resolve({
      ok: true,
      json: () =>
        Promise.resolve({
          name: 'France',
          capital: 'Paris',
          population: 67000000,
          flag: 'https://flagcdn.com/fr.png'
        })
    });
  }
  // Ensure fallback includes a json method to avoid undefined errors.
  return Promise.resolve({
    ok: false,
    json: () => Promise.resolve({})
  });
});

describe('Detail Page', () => {
  test('renders country details when data is fetched', async () => {
    render(
      <MemoryRouter initialEntries={['/country/France']}>
        <Routes>
          <Route path="/country/:name" element={<Detail />} />
        </Routes>
      </MemoryRouter>
    );

    // Wait for the country name to appear.
    await waitFor(() => expect(screen.getByText('France')).toBeInTheDocument());

    // Check that "Paris" appears somewhere in the Capital element.
    expect(screen.getByText(/Capital:/i)).toHaveTextContent(/Paris/);

    // Also verify the population appears.
    expect(screen.getByText(/Population:/i)).toHaveTextContent(/67,000,000/);
  });

  test('shows error when country not found', async () => {
    // For non-existent country, the mock returns ok: false.
    global.fetch.mockImplementationOnce(() =>
      Promise.resolve({
        ok: false,
        json: () => Promise.resolve({})
      })
    );
    render(
      <MemoryRouter initialEntries={['/country/NonExistent']}>
        <Routes>
          <Route path="/country/:name" element={<Detail />} />
        </Routes>
      </MemoryRouter>
    );
    // Wait for the error UI to appear.
    await waitFor(() => expect(screen.getByText(/Country not found/i)).toBeInTheDocument());
  });
});

// src/App.test.js
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders App component with loading indicator', () => {
  render(<App />);
  // Since the provider starts by showing "Loading...", check for that.
  expect(screen.getByText(/Loading/i)).toBeInTheDocument();
});

// frontend/src/pages/Detail.js
import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const Detail = () => {
  const { name } = useParams();
  const [countryDetails, setCountryDetails] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`http://localhost:5083/api/countries/${name}`)
      .then(response => {
        // If the response is 404, return null instead of throwing an error.
        if (response.status === 404) {
          return null;
        }
        if (!response.ok) {
          throw new Error('Error fetching country details');
        }
        return response.json();
      })
      .then(data => {
        setCountryDetails(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching country details:', error);
        setCountryDetails(null);
        setLoading(false);
      });
  }, [name]);

  if (loading) {
    return <div className="container text-center mt-5">Loading...</div>;
  }

  // When data is not available, display the placeholder image and a friendly message.
  if (!countryDetails) {
    return (
      <div className="container text-center mt-5">
        <img
          src="/placeholder.png"
          alt="Placeholder"
          className="img-fluid mb-3"
          style={{ maxWidth: '200px' }}
        />
        <h2>Country data not available.</h2>
        <p>Please try refreshing or check back later.</p>
        <Link to="/">Back to Home</Link>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <Link to="/" className="btn btn-secondary mb-3">← Back to Home</Link>
      <div className="card">
        <img
          src={countryDetails.flag}
          className="card-img-top"
          alt={`Flag of ${countryDetails.name}`}
          onError={e => { e.target.onerror = null; e.target.src = '/placeholder.png'; }}
        />
        <div className="card-body">
          <h2 className="card-title">{countryDetails.name}</h2>
          <p className="card-text"><strong>Capital:</strong> {countryDetails.capital}</p>
          <p className="card-text"><strong>Population:</strong> {countryDetails.population.toLocaleString()}</p>
        </div>
      </div>
    </div>
  );
};

export default Detail;

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
        if (!response.ok) throw new Error('Country not found');
        return response.json();
      })
      .then(data => {
        setCountryDetails(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching country details:', error);
        setLoading(false);
      });
  }, [name]);

  if (loading) {
    return <div className="container text-center mt-5">Loading...</div>;
  }

  if (!countryDetails) {
    return (
      <div className="container text-center mt-5">
        <h2>Country not found.</h2>
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

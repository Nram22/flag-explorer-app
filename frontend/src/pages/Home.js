// frontend/src/pages/Home.js
import React, { useContext } from 'react';
import { CountryContext } from '../context/CountryContext';
import { Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const Home = () => {
  const { countries, loading, error, refreshCountries } = useContext(CountryContext);

  if (loading) {
    return <div className="container text-center mt-5">Loading...</div>;
  }

  if (error) {
    return (
      <div className="container text-center mt-5">
        <img
          src="/placeholder.png"
          alt="Placeholder"
          className="img-fluid mb-3"
          style={{ maxWidth: '200px' }}
        />
        <h2>Unable to load country data.</h2>
        <p>Please check your network connection or try refreshing.</p>
        <button onClick={refreshCountries} className="btn btn-primary">
          Refresh
        </button>
      </div>
    );
  }

  return (
    <div className="container mt-3">
      <h1 className="mb-4">Flag Explorer</h1>
      <button onClick={refreshCountries} className="btn btn-primary mb-3">
        Refresh
      </button>
      <div className="row">
        {countries.map(country => (
          <div key={country.name} className="col-6 col-md-3 mb-4">
            <div className="card">
              <Link to={`/country/${encodeURIComponent(country.name)}`}>
                <img
                  src={country.flag}
                  className="card-img-top"
                  alt={`Flag of ${country.name}`}
                  onError={e => { e.target.onerror = null; e.target.src = '/placeholder.png'; }}
                />
              </Link>
              <div className="card-body">
                <h5 className="card-title text-center">{country.name}</h5>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Home;

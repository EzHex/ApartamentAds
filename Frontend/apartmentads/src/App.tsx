import React from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Navbar } from './components/Navbar';
import { Footer } from './components/Footer';
import { Route, Routes } from 'react-router-dom';
import { Ads } from './components/pages/Ads';
import { Home } from './components/pages/Home';
import { Login } from './components/pages/Login';
import { Register } from './components/pages/Register';

function App() {
  return (
    <div className='App'>
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/ads' element={<Ads />} />
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
      </Routes>
      <Footer />
    </div>
  );
}

export default App;

import React from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Navbar } from './components/Navbar';
import { Footer } from './components/Footer';
import { Route, Routes } from 'react-router-dom';
import { Ads } from './components/pages/Ad/Ads';
import { Home } from './components/pages/Home';
import { Login } from './components/pages/Login';
import { Register } from './components/pages/Register';
import Apartments from './components/pages/Apartment/Apartments';
import CreateApartment from './components/pages/Apartment/CreateApartment';
import EditApartment from './components/pages/Apartment/EditApartment';
import ViewApartment from './components/pages/Apartment/ViewApartment';
import CreateRoom from './components/pages/Room/CreateRoom';
import ViewRoom from './components/pages/Room/ViewRoom';
import CreateObject from './components/pages/Object/CreateObject';
import ViewObject from './components/pages/Object/ViewObject';
import NotFound from './components/pages/NotFound';
import ViewAd from './components/pages/Ad/ViewAd';

function App() {
  return (
    <div className='App'>
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/ads' element={<Ads />} />
        <Route path='/ads/:id' element={<ViewAd />} />
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route path='/apartments' element={<Apartments />} />
        <Route path='/apartments/:id' element={<ViewApartment />} />
        <Route path='/create-apartment' element={<CreateApartment />} />
        <Route path='/edit-apartment/:id' element={<EditApartment />} />
        <Route path='/apartments/:apartmentId/create-room' element={<CreateRoom />} />
        <Route path='/apartments/:apartmentId/rooms/:id' element={<ViewRoom />} />
        <Route path='/apartments/:apartmentId/rooms/:roomId/create-object' element={<CreateObject/>} />
        <Route path='/apartments/:apartmentId/rooms/:roomId/objects/:id' element={<ViewObject />} />

        <Route path='*' element={<NotFound />} />
      </Routes>
      <Footer />
    </div>
  );
}

export default App;

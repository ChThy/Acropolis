import { useEffect, useState } from 'react'
import './App.css'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages } from './store/actions';
import { pagesSelector } from './store/selectors';
import Pages from './components/Pages';

function App() {
  const dispatch = useAppDispatch();

  const pages = useAppSelector(pagesSelector)

  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
  }, []);

  return (
    <>
      <Pages pages={pages} />
    </>
  )
}

export default App

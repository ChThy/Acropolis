import { useEffect, useState } from 'react'
import './App.css'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages } from './store/actions';
import { pagesSelector } from './store/selectors';

function App() {
  const dispatch = useAppDispatch();

  const pages = useAppSelector(pagesSelector)

  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
  }, []);

  return (
    <>
      <p>Total pages scraped: {pages.length}</p>
      <div>
        {pages.map(page => (
          <>
            <p key={page.url} >{page.url}</p>
          </>
        ))}
      </div>
    </>
  )
}

export default App

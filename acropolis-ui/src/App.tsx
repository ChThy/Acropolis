import { useEffect, useState } from 'react'
import './App.css'
import { useAppDispatch } from './store/hooks'
import { fetchPages } from './store/actions';

function App() {
  const [count, setCount] = useState(0)

  const dispatch = useAppDispatch();

  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
  }, []);



  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          {/* <img src={viteLogo} className="logo" alt="Vite logo" /> */}
        </a>
        <a href="https://react.dev" target="_blank">
          {/* <img src={reactLogo} className="logo react" alt="React logo" /> */}
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App

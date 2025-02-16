import './App.css'
import { useEffect, useState } from 'react'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages, fetchVideos } from './store/actions';
import { pagesSelector, videosSelector } from './store/selectors';
import ResourceList from './components/ResourceList';
import Drawer from './components/Drawer';

function App() {
  const dispatch = useAppDispatch();

  const pages = useAppSelector(pagesSelector)
  const videos = useAppSelector(videosSelector);

  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
    dispatch(fetchVideos());
  }, []);

  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className='container mx-auto max-w-max'>
      <button type='button' onClick={() => setIsOpen(!isOpen)}>Toggle drawer</button>
      <Drawer isOpen={isOpen} position='left' onClose={() => setIsOpen(false)}>
        <div>

          Hello, this is something in the drawer
          <button type="button" onClick={() => setIsOpen(false)}>
            Close
          </button>
        </div>
      </Drawer>
      <div className='grid grid-flow-col auto-cols-fr'>
        <ResourceList items={pages} />
        <ResourceList items={videos} />
      </div>
    </div>
  )
}

export default App

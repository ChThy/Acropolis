import './App.css'
import { useEffect } from 'react'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages, fetchVideos } from './store/actions';
import { pagesSelector, videosSelector } from './store/selectors';
import ResourceList from './components/ResourceList';

function App() {
  const dispatch = useAppDispatch();

  const pages = useAppSelector(pagesSelector)
  const videos = useAppSelector(videosSelector);

  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
    dispatch(fetchVideos());
  }, []);

  return (
    <div className='container mx-auto max-w-max'>
      <div className='grid grid-flow-col auto-cols-fr'>
        <ResourceList items={pages} />
        <ResourceList items={videos} />
      </div>
    </div>
  )
}

export default App

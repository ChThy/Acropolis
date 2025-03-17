import './App.css'
import 'bootstrap-icons/font/bootstrap-icons.css';

import { useEffect, useState } from 'react'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages, fetchVideos } from './store/thunks.ts';
import { pagesSelector, videosSelector } from './store/selectors';
import ResourceList from './components/ResourceList';
import Drawer from './components/drawer/Drawer';
import { Button } from 'antd';
import { MainLayout } from './components/layout/MainLayout';
import { routes } from './routes.tsx';
import { useRoutes } from 'react-router';

export function App() {
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
    <>
      <MainLayout />
    </>
  );

  return (
    <div className='container mx-auto max-w-max'>

      <Drawer isOpen={isOpen} position='right' onClose={() => setIsOpen(false)}>
        <div>
          <p>Something inside the drawer</p>

          <Button variant='solid' color='danger' onClick={() => setIsOpen(false)}>
            Close
          </Button>
        </div>
      </Drawer>

      <div className='grid grid-flow-col auto-cols-fr'>
        <ResourceList items={pages} />
        <ResourceList items={videos} />
      </div>
    </div>
  )
}

export default App;

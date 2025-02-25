import './App.css'
import 'bootstrap-icons/font/bootstrap-icons.css';

import { useEffect, useState } from 'react'
import { useAppDispatch, useAppSelector } from './store/hooks'
import { fetchPages, fetchVideos } from './store/actions';
import { pagesSelector, videosSelector } from './store/selectors';
import ResourceList from './components/ResourceList';
import Drawer from './components/drawer/Drawer';
import { Button, Card } from 'antd';
import { DeleteFilled } from '@ant-design/icons';
import { resourcesActions, useResourcesDispatch, useThunkDispath,  } from './store/resource-slice';


function App() {
  const dispatch = useAppDispatch();
  const thunkDispatch = useThunkDispath();
  const resourcesDispatch = useResourcesDispatch();

  const pages = useAppSelector(pagesSelector)
  const videos = useAppSelector(videosSelector);



  useEffect(() => {
    console.log('dispatching');
    dispatch(fetchPages());
    dispatch(fetchVideos());
    dispatch(resourcesActions.add());

    thunkDispatch(fetchPages());
    thunkDispatch(fetchVideos());
    thunkDispatch(resourcesActions.add());

    resourcesDispatch(fetchPages());
    resourcesDispatch(fetchVideos());
    resourcesDispatch(resourcesActions.add());
    
  }, []);

  const [isOpen, setIsOpen] = useState(false);
  

  return (
    <div className='container mx-auto max-w-max'>

      <i className="bi-alarm" />
      <Button type='primary' onClick={() => setIsOpen(!isOpen)}>Toggle drawer</Button>

      <Card
        actions={[
          <Button>
            <DeleteFilled style={{ color: "red" }  } />
          </Button>
        ]}
      >

      </Card>

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

export default App

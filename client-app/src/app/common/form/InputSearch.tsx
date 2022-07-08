import { observer } from 'mobx-react-lite';
import React, { useState } from 'react'
import { Button, Icon, Input } from 'semantic-ui-react'
import { useStore } from '../../stores/store';

export default observer(function InputExampleIconElement () { 
    const {activityStore: {setPredicate}} = useStore();
      const [textInput, setTextInput] = useState('');
    
    const handleClick = () => {
      console.log(textInput);
      setPredicate('coinname', textInput);
    }
  
    const handleChange = (event: any) => {
      setTextInput(event.target.value);
      setPredicate('all', 'true')
    }

return(
  <Input
    className='searchInput'
    icon={<Button name='search' positive size='small' content='Search' onClick={handleClick}/>}
    onChange={handleChange}
    placeholder='Search...' />   
)});


import './App.css';
import { Container, AppBar, Toolbar, Typography } from '@mui/material';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';

import ReviewsPage from './ReviewsPage';

function App() {
  return (
    <Router>
      <AppBar position="static" className='AppBar'>
        <Container>
          <Toolbar disableGutters={true}>
            <Link to="/">
              <Typography variant="h6" component="div">
                FrontEnd
              </Typography>
            </Link>
            <Link to="/reviews" ><Typography>Reviews</Typography></Link>
          </Toolbar>
        </Container>
      </AppBar>
      <Container>
        <Routes>
          <Route path={"/"} />
          <Route path="/reviews" element={<ReviewsPage />} />
        </Routes>
      </Container>
    </Router>
  );
}
export default App;
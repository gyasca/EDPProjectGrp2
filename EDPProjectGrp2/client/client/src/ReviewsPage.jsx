// src/pages/ReviewsPage.jsx
import React, { useEffect, useState } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import axios from 'axios';

const ReviewsPage = () => {
    const [reviews, setReviews] = useState([]);

    useEffect(() => {
        // Fetch reviews from your ASP.NET MVC API
        axios.get('https://localhost:7261/Reviews')
            .then(response => setReviews(response.data))
            .catch(error => console.error('Error fetching reviews:', error));
    }, []);

    const columns = [
        { field: 'id', headerName: 'ID', width: 70 },
        { field: 'userName', headerName: 'User Name', width: 150 },
        { field: 'comment', headerName: 'Comment', flex: 1 },
    ];

    return (
        <div style={{ height: 400, width: '100%' }}>
            <DataGrid rows={reviews} columns={columns} pageSize={5} />
        </div>
    );
};

export default ReviewsPage;

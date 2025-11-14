<script setup>
import { ref, onMounted, computed, watch } from 'vue';
import axios from 'axios';

const API_URL_ALL = 'http://localhost:5032/formster/api/form-submission/all';
const API_URL_SEARCH = 'http://localhost:5032/formster/api/form-submission/search';

const PAGE_SIZE = 10; 

const allSubmissions = ref([]);
const isLoading = ref(false);

const searchFormName = ref('');
const searchQuery = ref('');
const isSearching = ref(false);

const currentPage = ref(1);
const pageSize = ref(PAGE_SIZE);

const totalPages = computed(() => {
    return Math.ceil(allSubmissions.value.length / pageSize.value);
});

const paginatedSubmissions = computed(() => {
    const skip = (currentPage.value - 1) * pageSize.value;
    const take = pageSize.value;
    
    return allSubmissions.value.slice(skip, skip + take);
});

const pageNumbers = computed(() => {
    const pages = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(1, currentPage.value - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(totalPages.value, startPage + maxPagesToShow - 1);

    if (endPage - startPage + 1 < maxPagesToShow) {
        startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
    }
    return pages;
});

const fetchSubmissions = async (isNewSearch = false) => {
    isLoading.value = true;
    
    if (isNewSearch) {
        currentPage.value = 1;
    }

    const isSearchMode = searchFormName.value || searchQuery.value;
    isSearching.value = !!isSearchMode;
    
    let url = isSearchMode ? API_URL_SEARCH : API_URL_ALL;
    let params = {};
    
    if (isSearchMode) {
        params = {
            formName: searchFormName.value || '',
            query: searchQuery.value || '',
        };
    } else {
        params = {
            skip: PAGE_SIZE * (currentPage.value - 1), 
            take: 10
        };
    }

    try {
        const response = await axios.get(url, { params });

        allSubmissions.value = response.data.map(sub => ({
            ...sub,
            // parsedJsonData: sub.jsonData ? JSON.parse(sub.jsonData) : {}
        }));

    } catch (error) {
        console.error('Error fetching submissions:', error.response ? error.response.data : error.message);
        alert(`Failed to load data from ${url}. Check API connection.`);
    } finally {
        isLoading.value = false;
    }
};

const changePage = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages.value) {
        currentPage.value = newPage;
    }
};

const handleClear = () => {
    searchFormName.value = '';
    searchQuery.value = '';
    fetchSubmissions(true);
};

onMounted(() => {
    fetchSubmissions(true);
});

</script>

<template>
    <div class="submissions-list-container">
        <h2>
            {{ isSearching ? 'Search Results' : 'All Form Submissions' }} 
            ({{ allSubmissions.length }} total)
        </h2>

        <div class="search-panel">
            <input type="text" v-model="searchFormName" placeholder="Search by Form Name..." @keyup.enter="fetchSubmissions(true)" />
            <input type="text" v-model="searchQuery" placeholder="Search by Data Value (e.g., '20')" @keyup.enter="fetchSubmissions(true)" />
            
            <button @click="fetchSubmissions(true)" :disabled="isLoading">Search</button>
            <button @click="handleClear">Clear</button>
        </div>

        <div v-if="isLoading" class="loading-state">Loading submissions...</div>
        <div v-else-if="allSubmissions.length === 0 && !isLoading" class="no-results">No submissions found.</div>

        <table v-else class="submissions-table">
            <thead>
                </thead>
            <tbody>
                <tr v-for="sub in paginatedSubmissions" :key="sub.id"> 
                    <td>{{ sub.id.substring(0, 8) }}...</td>
                    <td>{{ sub.formName }}</td>
                    <td class="json-data">
                        <pre>{{ sub.jsonData }}</pre>
                    </td>
                    <td>{{ new Date(sub.createdAt).toLocaleString() }}</td>
                </tr>
            </tbody>
        </table>

        <div v-if="totalPages > 1" class="pagination-controls">
            </div>
    </div>
</template>
import { useEffect, useRef, useState } from "react"
import { SESSION_STORAGE_USER_KEY } from "../../constants";
import { useNavigate } from "react-router-dom";
import { Content } from "../../components/Content";
import { getLoggedUser } from "../../helpers/helper";
import LogoutIcon from '@mui/icons-material/Logout';
import { UserInfoContainer } from "./styles";
import { PostInput } from "../../components/PostInput";
import { PosterrRecord } from "../../models/Record";
import { fetchRecords } from "../../services/RecordService";
import { FilterInput } from "../../components/FilterInput";
import { getPostsByKeywords } from "../../services/PostService";

export const Home = () => {
    /*====================== STATES ======================*/
    const [records, setRecords] = useState<PosterrRecord[]>([]);
    const [hasMore, setHasMore] = useState<boolean>(true);
    
    // useRef to properly deal with value changing whitin handleScroll function
    const isLoading = useRef(false); 
    const scrollPageCounterRef = useRef(0);
    const sortByDateRef = useRef(true);
    const filterTextRef = useRef("");

    const navigate = useNavigate();

    /*====================== EFFECTS ======================*/
    useEffect(() => {
        // Does not allow to load Homepage if there is no user selected
        if (!checkCurrentUser()) navigate("/");

        // Attach scroll event to load more data (date sort, trend sort and filtered posts)
        window.addEventListener("scroll", handleScroll);
        fetchData(0);
        
        return () => {
          window.removeEventListener("scroll", handleScroll);
        };
    }, []);

    /*====================== FETCHES ======================*/
    const fetchData = (page: number = 0) => {
        if (isLoading.current || !hasMore) return;
        isLoading.current = true;

        // Fetch data from default endpoint or the filtered one (if posts are filtered by keywords)
        if (!filterTextRef.current){
            fetchRecords(page, sortByDateRef.current)
                .then((response: PosterrRecord[]) => {
                    if (response.length === 0) {
                        setHasMore(false);
                        return;
                    }
                    setRecords((prevRecords) => [...prevRecords, ...response]);
                    scrollPageCounterRef.current = page;
                })
                .finally(() => isLoading.current = false);
        }else {
            getPostsByKeywords(filterTextRef.current, page)
                .then((response: PosterrRecord[]) => {
                    if (response.length === 0) {
                        setHasMore(false);
                        return;
                    }
                    setRecords((prevRecords) => [...prevRecords, ...response]);
                    scrollPageCounterRef.current = page;
                })
                .finally(() => isLoading.current = false);
        }
    }

    const filterPostsByKeywords = (keywords: string, page: number = 0) => {
        if (isLoading.current || !hasMore) return;
        isLoading.current = true;
        
        getPostsByKeywords(keywords, page)
            .then((response: PosterrRecord[]) => {
                if (response.length === 0) {
                    setHasMore(false);
                    return;
                }
                setRecords((prevRecords) => [...prevRecords, ...response]);
                scrollPageCounterRef.current = page;
            })
            .finally(() => isLoading.current = false);
    }

    /*====================== FUNCTIONS ======================*/
    const clearData = () => {
        setRecords([]);
        scrollPageCounterRef.current = 0;
        sortByDateRef.current = true;
        setHasMore(true);
    }

    const changeSortData = () => {
        setRecords([]);
        scrollPageCounterRef.current = 0;
        setHasMore(true);
        fetchData();
    }

    const handleScroll = () => {
        const bottom = Math.abs((window.innerHeight + document.documentElement.scrollTop) - document.documentElement.offsetHeight) < 10;
        if (bottom && !isLoading.current) {
            fetchData(scrollPageCounterRef.current + 1);
        }
    };

    const checkCurrentUser = () => {
        const sessionUser = sessionStorage.getItem(SESSION_STORAGE_USER_KEY);
        if (!sessionUser) return false

        return true
    }

    const logout = () => {
        sessionStorage.removeItem(SESSION_STORAGE_USER_KEY);
        navigate("/");
    }

    /*====================== RENDER ======================*/
    return <div>
        <UserInfoContainer>
            <span>Logged in as @{getLoggedUser()}</span>
            <LogoutIcon onClick={() => logout()}/>
        </UserInfoContainer>

        <FilterInput
            filterPostsByKeywords={filterPostsByKeywords} 
            filterTextRef={filterTextRef}
            clearData={clearData}
        />

        <PostInput 
            clearData={clearData} 
            fetchData={fetchData} 
            filterTextRef={filterTextRef}
        />
        
        <Content 
            records={records} 
            clearData={clearData} 
            fetchData={fetchData} 
            sortByDateRef={sortByDateRef}
            changeSortData={changeSortData}
        />
    </div>
}

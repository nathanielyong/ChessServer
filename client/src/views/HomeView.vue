<template>
    <div class="home">
        <div v-if='isAuthenticated'>
            <LiveChessRequest />
        </div>
        <div v-else>
            <h1>Welcome to Chess Arena! Login to create a challenge and start playing</h1>
        </div>
    </div>
</template>
<script>
import LiveChessRequest from '@/components/LiveChessRequest.vue'
import { mapGetters, mapActions } from 'vuex';

export default {
    data() {
        return {
            username: '',
            password: '',
            errorMessage: ''
        }
    },
    computed: {
        ...mapGetters(['isAuthenticated', 'getToken']),
    },
    async mounted() {

    },
    components: {
        LiveChessRequest
    }
}
</script>
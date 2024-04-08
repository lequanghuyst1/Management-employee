<template>
    <div class="m-loading-table">
            <div class="m-loading-table__icon">
                <div></div>
                <div></div>
                <div></div>
                <div></div>
            </div>
        </div>
</template>
<script>
export default {
    name: 'MLoadingData'
}
</script>
<style scoped>
.m-loading-table {
    width: 100%;
    height: calc(100% - 48px);
    position: sticky;
    left: 0;
    background-color: white;
    z-index: 3;
}
.m-loading-table__icon {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    display: inline-block;
    width: 80px;
    height: 80px;
}
.m-loading-table__icon div {
    box-sizing: border-box;
    display: block;
    position: absolute;
    width: 64px;
    height: 64px;
    margin: 8px;
    border: 8px solid #2CA01C;
    border-radius: 50%;
    animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
    border-color: #2CA01C transparent transparent transparent;
}
.m-loading-table__icon div:nth-child(1) {
    animation-delay: -0.45s;
}
.m-loading-table__icon div:nth-child(2) {
    animation-delay: -0.3s;
}
.m-loading-table__icon div:nth-child(3) {
    animation-delay: -0.15s;
}
@keyframes lds-ring {
    0% {
        transform: rotate(0deg);
    }
    100% {
        transform: rotate(360deg);
    }
}

</style>
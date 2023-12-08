using Godot;


public static class RoomRequester {
        public static Level currentLevel;


        public static RoomLoader GetLoader() {
                return new RoomLoader();
        }
}